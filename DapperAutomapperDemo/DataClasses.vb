Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema



Partial Public Class Destination
	Public Property Phones As List(Of Phones)
End Class

<Table("DataSource")>
Partial Public Class DataSource
	<Key()>
	<Column("SRC_ID")>
	<DatabaseGenerated(DatabaseGeneratedOption.Identity)>
	Public Property SRC_ID As Decimal

	<Column("Title")>
	<MaxLength(50)>
	Public Property Title As String

	<Column("Firstname")>
	<MaxLength(100)>
	Public Property Firstname As String

	<Column("Lastname")>
	<MaxLength(100)>
	Public Property Lastname As String

	<Column("DOB")>
	<DataType(DataType.Date)>
	Public Property DOB As System.Nullable(Of System.DateTime)

	<Column("MobileNumber")>
	<MaxLength(20)>
	Public Property MobileNumber As String

	<Column("Telephone")>
	<MaxLength(20)>
	Public Property Telephone As String

	<Column("email")>
	<MaxLength(100)>
	Public Property email As String
End Class

<Table("Destination")>
Partial Public Class Destination
	<Key()>
	<Column("DST_ID")>
	<DatabaseGenerated(DatabaseGeneratedOption.Identity)>
	Public Property DST_ID As Decimal

	<Column("Forename")>
	<MaxLength(100)>
	Public Property Forename As String

	<Column("Surname")>
	<MaxLength(50)>
	Public Property Surname As String

	<Column("DateOfBirth")>
	<DataType(DataType.Date)>
	Public Property DateOfBirth As System.Nullable(Of System.DateTime)

	<Column("Status")>
	<MaxLength(10)>
	Public Property Status As String

	<Column("CreateDate")>
	<MaxLength(10)>
	Public Property CreateDate As String

	<Column("Prefix")>
	<MaxLength(14)>
	Public Property Prefix As String

	<Column("Position")>
	<MaxLength(50)>
	Public Property Position As String

End Class

<Table("Phones")>
Partial Public Class Phones
	<Key()>
	<Column("CNP_ID")>
	<DatabaseGenerated(DatabaseGeneratedOption.Identity)>
	Public Property CNP_ID As Decimal

	<Column("NumberType")>
	<MaxLength(50)>
	Public Property NumberType As String

	<Column("PhoneNumber")>
	<MaxLength(10)>
	Public Property PhoneNumber As String

	<Column("DST_ID")>
	Public Property DST_ID As Long
End Class