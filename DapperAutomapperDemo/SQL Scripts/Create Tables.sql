USE [YOURDATABASENAME]
GO
/****** Object:  Table [dbo].[DataSource]    Script Date: 2021/04/27 10:39:35 ******/
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
/****** Object:  Table [dbo].[Destination]    Script Date: 2021/04/27 10:39:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Destination](
	[DST_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Forename] [varchar](100) NULL,
	[Surname] [varchar](50) NULL,
	[DateOfBirth] [date] NULL,
	[Status] [varchar](100) NULL,
	[CreateDate] [datetime] NULL,
	[Prefix] [varchar](14) NULL,
	[Position] [varchar](50) NULL,
 CONSTRAINT [PK_Destination] PRIMARY KEY CLUSTERED 
(
	[DST_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Phones]    Script Date: 2021/04/27 10:39:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Phones](
	[CNP_ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[NumberType] [varchar](50) NULL,
	[PhoneNumber] [varchar](20) NULL,
	[DST_ID] [bigint] NULL,
 CONSTRAINT [PK_Phones] PRIMARY KEY CLUSTERED 
(
	[CNP_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
