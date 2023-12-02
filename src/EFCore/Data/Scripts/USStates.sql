/****** Object: USStates        Script Date: 09/27/2023 ******/
CREATE TABLE [dbo].[USStates](
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [Alpha] [nvarchar](2) NULL,
  [Name] [nvarchar](50) NOT NULL,
  [Capital] [nvarchar](50) NULL,
 CONSTRAINT [PK_USStates] PRIMARY KEY CLUSTERED 
(
  [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_USStates_Alpha] ON [dbo].[USStates]
(
  [Alpha] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

/****** USStates.txt: 54 rows ******/
/****** including DC = District of Columbia, PR = Puerto Rico, XX = Unknown, and NULL = Undefined) ******/