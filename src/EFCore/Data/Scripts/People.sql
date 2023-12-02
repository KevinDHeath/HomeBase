/****** Object: People    Script Date: 09/27/2023 ******/
CREATE TABLE [dbo].[People](
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [FirstName] [nvarchar](50) NOT NULL,
  [MiddleName] [nvarchar](50) NULL,
  [LastName] [nvarchar](50) NOT NULL,
  [Address_Street] [nvarchar](50) NULL,
  [Address_City] [nvarchar](50) NULL,
  [Address_State] [nvarchar](50) NULL,
  [Address_ZipCode] [nvarchar](20) NULL,
  [Address_Country] [nvarchar](50) NULL,
  [GovernmentNumber] [nvarchar](50) NULL,
  [IdState] [nvarchar](50) NULL,
  [IdNumber] [nvarchar](50) NULL,
  [HomePhone] [nvarchar](50) NULL,
  [BirthDate] [date] NULL,
 CONSTRAINT [PK_People] PRIMARY KEY CLUSTERED 
(
  [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** People.txt: 135 rows ******/