Imports System
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Data.SqlClient
Imports Dapper
Imports AutoMapper
Imports System.Data
Imports System.Net.Mail

' This code does not use much of the extended functionality built into AutoMapper
' With time I may extend this. If you have code or an examples to include please let me know.
' 

Module Program
    Private _db As IDbConnection

    ' I did some tests on another project, the movement of the mapper set up to global took the run time from
    ' 180 sec to 4.16. RTFM they tell you to make this global for a reason.
    '
    ' To debug you may want to move it to the Move mapper config to here line, for production
    ' make sure that it is globally configured.

    Private ReadOnly DestMapperConfig As New MapperConfiguration(Sub(config)
                                                                     config = DatasourceMapConfig(config)
                                                                 End Sub)
    Sub Main()
        Console.WriteLine("Dapper & AutoMapper Demo")
        Console.WriteLine("https://github.com/TCBWZA/DapperAutomapperDemo")

        Dim lDatasource As New List(Of DataSource)
        Dim lDestination As New List(Of Destination)
        Dim DST_ID As Long

        ' Move mapper config to here


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



        Try

            ' The line below will list all the unmapped fields. Use this to debug and trouble shoot
            ' This will list all the fields that are not mapped or ignored.
            ' This makes checking your code very quick and easy.
            ' TRY IT (The demo done not have any errors, I suggest adding a random field to the Destination Class.
            ' VVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVV
            ' DestMapperConfig.AssertConfigurationIsValid()


            Dim DestMapper = DestMapperConfig.CreateMapper


            DestItem = DestMapper.Map(Of Destination)(iRec)
        Catch ex As AutoMapperMappingException
            Throw
        Catch ex As Exception
            If ex.Source.ToUpper = "AUTOMAPPER" Then
                Throw
            End If
        End Try

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
        ' This way of returning the inserted ID should only be used where there are no triggers. Triggers affect the buffer post the Inserted. stage.
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
        ' If the source and destination field names are the same automapper will map them automatically
        ' These examples are for changing the mapping where they are different.
        ' If you need a rule for this e.g. do not map if string is empty you can use the following
        ' Function(opt) opt.Condition(Function(src As DataSource) (src.Forename <> "")


        Config.CreateMap(Of DataSource, Destination)() _
                    .ForMember(Function(dest) dest.Status, Sub(opt) opt.MapFrom(Function(src) StaticValue)) _
                    .ForMember(Function(dest) dest.CreateDate, Sub(opt) opt.MapFrom(Function(src) DateTime.UtcNow)) _
                    .ForMember(Function(dest) dest.Forename, Sub(opt) opt.MapFrom(Function(src As DataSource) src.Firstname)) _
                    .ForMember(Function(dest) dest.Surname, Sub(opt) opt.MapFrom(Function(src As DataSource) src.Lastname)) _
                    .ForMember(Function(dest) dest.DateOfBirth, Sub(opt) opt.MapFrom(Function(src As DataSource) src.DOB)) _
                    .ForMember(Function(dest) dest.Prefix, Sub(opt) opt.MapFrom(Function(src As DataSource) If(src.Title.Length < 7, src.Title, Nothing))) _
                    .ForMember(Function(dest) dest.Position, Sub(opt) opt.MapFrom(Function(src As DataSource) If(src.Title.Length >= 7, src.Title, Nothing))) _
                    .ForMember(Function(dest) dest.Phones, Sub(opt) opt.MapFrom(Of PhoneResolver)())

        ' Should you wish to ignore specific fields then you can use the following.
        '.ForMember(Function(dest) dest.Phones, Sub(opt) opt.Ignore())

        Return Config
    End Function
    Function MobileMapConfig(Config As IMapperConfigurationExpression, Optional DST_ID As Long = -1) As IMapperConfigurationExpression
        ' Just an example with minimal mapping with a conditional check.
        Config.CreateMap(Of DataSource, Phones)() _
                    .ForMember(Function(dest) dest.NumberType, Sub(opt) opt.MapFrom(Function(src) "Mobile")) _
                    .ForMember(Function(dest) dest.DST_ID, Sub(opt) opt.MapFrom(Function(src) DST_ID)) _
                    .ForMember(Function(dest) dest.PhoneNumber, Sub(opt) opt.MapFrom(Function(src As DataSource) If(src.MobileNumber.Length > 8, src.MobileNumber, Nothing)))
        Return Config
    End Function

    ' Map the different telephone numbers from the Source into a Phones List on the destination
    Public Class PhoneResolver
        Implements IValueResolver(Of DataSource, Destination, List(Of Phones))

        Public Function Resolver(ByVal source As DataSource, ByVal destination As Destination, ByVal destMember As List(Of Phones), ByVal context As ResolutionContext) As List(Of Phones) Implements IValueResolver(Of DataSource, Destination, List(Of Phones)).Resolve
            Dim result As List(Of Phones) = New List(Of Phones)()

            If Not String.IsNullOrEmpty(source.MobileNumber) Then
                result.Add(New Phones With {
                    .PhoneNumber = source.MobileNumber,
                    .NumberType = "Mobile"
                })
            End If

            If Not String.IsNullOrEmpty(source.Telephone) Then
                result.Add(New Phones With {
                    .PhoneNumber = source.Telephone,
                    .NumberType = "Telephone"
                })
            End If

            Return result
        End Function
    End Class

End Module
