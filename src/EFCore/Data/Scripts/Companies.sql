/****** Object: Companies    Script Date: 09/27/2023 ******/
CREATE TABLE [dbo].[Companies](
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [Name] [nvarchar](100) NOT NULL,
  [Address_Street] [nvarchar](50) NULL,
  [Address_City] [nvarchar](50) NULL,
  [Address_State] [nvarchar](50) NULL,
  [Address_ZipCode] [nvarchar](20) NULL,
  [Address_Country] [nvarchar](50) NULL,
  [GovernmentNumber] [nvarchar](50) NULL,
  [PrimaryPhone] [nvarchar](50) NULL,
  [SecondaryPhone] [nvarchar](50) NULL,
  [Email] [nvarchar](50) NULL,
  [NaicsCode] [nvarchar](20) NULL,
  [Private] [char](1) NULL,
  [DepositsCount] [int] NULL,
  [DepositsBal] [decimal](18, 2) NULL,
 CONSTRAINT [PK_Companies] PRIMARY KEY CLUSTERED 
(
  [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Companies.txt: 135 rows ******/