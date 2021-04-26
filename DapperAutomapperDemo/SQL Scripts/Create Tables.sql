USE [YourDatabasename]
GO
/****** Object:  Table [dbo].[DataSource]    Script Date: 2021/04/26 16:26:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataSource](
	[SRC_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Title] [varchar](50) NULL,
	[Firstname] [varchar](100) NULL,
	[Lastname] [varchar](100) NULL,
	[DOB] [date] NULL,
	[MobileNumber] [varchar](20) NULL,
	[Telephone] [varchar](20) NULL,
	[email] [varchar](100) NULL,
 CONSTRAINT [PK_DataSource] PRIMARY KEY CLUSTERED 
(
	[SRC_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Destination]    Script Date: 2021/04/26 16:26:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Destination](
	[DST_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Forename] [varchar](100) NULL,
	[Surname] [varchar](50) NULL,
	[DateOfBirth] [date] NULL,
 CONSTRAINT [PK_Destination] PRIMARY KEY CLUSTERED 
(
	[DST_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Phones]    Script Date: 2021/04/26 16:26:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Phones](
	[CNP_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[NumberType] [varchar](50) NULL,
	[PhoneNumber] [nchar](10) NULL,
	[DST_ID] [nchar](10) NULL,
 CONSTRAINT [PK_Phones] PRIMARY KEY CLUSTERED 
(
	[CNP_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
