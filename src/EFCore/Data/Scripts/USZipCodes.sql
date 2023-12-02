/****** Object: USZipCodes      Script Date: 09/27/2023 ******/
CREATE TABLE [dbo].[USZipCodes](
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [State] [nvarchar](2) NOT NULL,
  [ZipCode] [nvarchar](5) NOT NULL,
  [County] [nvarchar](50) NOT NULL,
  [City] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_USZipCodes] PRIMARY KEY CLUSTERED 
(
  [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_USZipCodes_ZipCode] ON [dbo].[USZipCodes]
(
  [ZipCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

/****** USZipCodes.txt: 158 rows (used by the Companies and People data) ******/
/****** USZipCodes_full.txt: 41,755 rows (All US Zip Codes) ******/