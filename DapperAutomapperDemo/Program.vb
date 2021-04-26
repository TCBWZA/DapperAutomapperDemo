Imports System
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Data.SqlClient
Imports Dapper
Imports AutoMapper
Imports System.Data

Module Program
    Private _db As IDbConnection

    Sub Main(args As String())
        Console.WriteLine("Dapper & AutoMapper Demo")
        Dim lDatasource As New List(Of DataSource)
        Dim lDestination As New List(Of Destination)
        Dim DST_ID As Long


        lDatasource = GetSourcedata()

        For Each iRec As DataSource In lDatasource
            Dim iDestination As New Destination

            iDestination = MapRecord(iRec)

            lDestination.Add(iDestination)
        Next

        For Each iDestination As Destination In lDestination
            DST_ID = Insertdata(iDestination)
        Next

    End Sub

    Private Function MapRecord(iRec As DataSource) As Destination
        Dim DestItem As New Destination
        Dim MobItem As New Phones
        Dim TeleItem As New Phones
        Dim lPhones As New List(Of Phones)

        Dim DestMapperConfig As New MapperConfiguration(Sub(config)
                                                            config = DatasourceMapConfig(config)
                                                        End Sub)
        Dim MobileMapperConfig As New MapperConfiguration(Sub(config)
                                                              config = MobileMapConfig(config)
                                                          End Sub)
        Dim PhoneMapperConfig As New MapperConfiguration(Sub(config)
                                                             config = PhoneMapConfig(config)
                                                         End Sub)


        Try

            ' The line below will list all the unmapped fields. Use this to debug and trouble shoot
            ' DestMapperConfig.AssertConfigurationIsValid()


            Dim DestMapper = DestMapperConfig.CreateMapper
            Dim MobMapper = MobileMapperConfig.CreateMapper
            Dim TelMapper = PhoneMapperConfig.CreateMapper


            DestItem = DestMapper.Map(Of Destination)(iRec)
            MobItem = MobMapper.Map(Of Phones)(iRec)
            TeleItem = TelMapper.Map(Of Phones)(iRec)
        Catch ex As AutoMapperMappingException
            Throw
        Catch ex As Exception
            If ex.Source.ToUpper = "AUTOMAPPER" Then
                Throw
            End If
        End Try
        If Not IsNothing(MobItem.PhoneNumber) Then
            lPhones.Add(MobItem)
        End If
        If Not IsNothing(TeleItem.PhoneNumber) Then
            lPhones.Add(TeleItem)
        End If


        If lPhones.Count > 0 Then
            DestItem.Phones = lPhones
        End If

        Return DestItem

    End Function

    Function GetSourcedata() As List(Of DataSource)
        ' Change the connection string to match your requirements
        _db = New SqlConnection("Data Source=.;Initial Catalog=DapperAutoMapperDemo;Integrated Security=True")
        Dim SQL As String = "Select * from Datasource"
        Return _db.Query(Of DataSource)(SQL, commandType:=CommandType.Text).ToList()

    End Function
    Function Insertdata(iDestination As Destination) As Long
        Dim DST_ID As Long
        Dim Parameters As New DynamicParameters
        ' Change the connection string to match your requirements
        _db = New SqlConnection("Data Source=.;Initial Catalog=DapperAutoMapperDemo;Integrated Security=True")
        Dim SQLDST As String = "Insert Into Destination (Forename,Surname,DateOfBirth,Status,CreateDate,Prefix,Position) OUTPUT Inserted.DST_ID " _
               & " Values(@Forename,@Surname,@DateOfBirth,@Status,@CreateDate,@Prefix,@Position)"

        Console.WriteLine(SQLDST)

        Parameters.Add("@Forename", iDestination.Forename)
        Parameters.Add("@Surname", iDestination.Surname)
        Parameters.Add("@DateOfBirth", iDestination.DateOfBirth)
        Parameters.Add("@Status", iDestination.Status)
        Parameters.Add("@CreateDate", iDestination.CreateDate)
        Parameters.Add("@Prefix", iDestination.Prefix)
        Parameters.Add("@Position", iDestination.Position)

        DST_ID = _db.ExecuteScalar(Of Long)(SQLDST, Parameters, commandType:=CommandType.Text)

        If iDestination.Phones.Count = 0 Then
            Return DST_ID
        End If

        Dim SQLPHN As String = "Insert Into PHONES (DST_ID,NumberType,PhoneNumber) Values(@DST_ID,@NumberType,@PhoneNumber)"

        For Each iPHN In iDestination.Phones
            Dim Param As New DynamicParameters
            Param.Add("@DST_ID", DST_ID)
            Param.Add("@NumberType", iPHN.NumberType)
            Param.Add("@PhoneNumber", iPHN.PhoneNumber)

            _db.Execute(SQLPHN, Param, commandType:=CommandType.Text)
        Next
        Return DST_ID
    End Function

    Function DatasourceMapConfig(Config As IMapperConfigurationExpression, Optional StaticValue As String = "StaticValue") As IMapperConfigurationExpression
        Config.CreateMap(Of DataSource, Destination)() _
                    .ForMember(Function(dest) dest.Status, Sub(opt) opt.MapFrom(Function(src) StaticValue)) _
                    .ForMember(Function(dest) dest.CreateDate, Sub(opt) opt.MapFrom(Function(src) DateTime.UtcNow)) _
                    .ForMember(Function(dest) dest.Forename, Sub(opt) opt.MapFrom(Function(src As DataSource) src.Firstname)) _
                    .ForMember(Function(dest) dest.Surname, Sub(opt) opt.MapFrom(Function(src As DataSource) src.Lastname)) _
                    .ForMember(Function(dest) dest.DateOfBirth, Sub(opt) opt.MapFrom(Function(src As DataSource) src.DOB)) _
                    .ForMember(Function(dest) dest.Prefix, Sub(opt) opt.MapFrom(Function(src As DataSource) If(src.Title.Length < 7, src.Title, Nothing))) _
                    .ForMember(Function(dest) dest.Position, Sub(opt) opt.MapFrom(Function(src As DataSource) If(src.Title.Length >= 7, src.Title, Nothing)))
        Return Config
    End Function
    Function MobileMapConfig(Config As IMapperConfigurationExpression, Optional DST_ID As Long = -1) As IMapperConfigurationExpression
        Config.CreateMap(Of DataSource, Phones)() _
                    .ForMember(Function(dest) dest.NumberType, Sub(opt) opt.MapFrom(Function(src) "Mobile")) _
                    .ForMember(Function(dest) dest.DST_ID, Sub(opt) opt.MapFrom(Function(src) DST_ID)) _
                    .ForMember(Function(dest) dest.PhoneNumber, Sub(opt) opt.MapFrom(Function(src As DataSource) src.MobileNumber))
        Return Config
    End Function
    Function PhoneMapConfig(Config As IMapperConfigurationExpression, Optional DST_ID As Long = -1) As IMapperConfigurationExpression
        Config.CreateMap(Of DataSource, Phones)() _
                    .ForMember(Function(dest) dest.NumberType, Sub(opt) opt.MapFrom(Function(src) "Telephone")) _
                    .ForMember(Function(dest) dest.DST_ID, Sub(opt) opt.MapFrom(Function(src) DST_ID)) _
                    .ForMember(Function(dest) dest.PhoneNumber, Sub(opt) opt.MapFrom(Function(src As DataSource) src.Telephone))
        Return Config
    End Function


End Module
