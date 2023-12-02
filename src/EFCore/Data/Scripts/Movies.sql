/****** Object: Movies    Script Date: 09/27/2023 1:15:56 PM ******/
CREATE TABLE [dbo].[Movies](
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [Title] [nvarchar](80) NOT NULL,
  [ReleaseYear] [int] NULL,
  [Genre] [nvarchar](100) NULL,
  [Duration] [int] NULL,
 CONSTRAINT [PK_Movies] PRIMARY KEY CLUSTERED 
(
  [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Movies_Title] ON [dbo].[Movies]
(
  [Title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

-- Renamed addRecord/createMovie as Movie_Insert
CREATE PROCEDURE [dbo].[Movie_Insert]
  @pTitle [nvarchar](80),
  @pReleaseYear int = null,
  @pGenre [nvarchar](100) = null,
  @pDuration int = null
AS
  INSERT INTO [Movies] ([Title], [ReleaseYear], [Genre], [Duration]) VALUES (@pTitle, @pReleaseYear, @pGenre, @pDuration);
GO

-- Renamed deleteRecord/deleteMovie as Movie_Delete
CREATE PROCEDURE [dbo].[Movie_Delete]
  @pId int
AS
  DELETE FROM [Movies] WHERE [Id] = @pId;
GO

-- Renamed retRecords/searchMovies as Movie_Search
CREATE PROCEDURE dbo.[Movie_Search]
  @pTitlePhrase [nvarchar](80)
AS
  DECLARE @param [nvarchar](82); SET @param = '%' + @pTitlePhrase + '%'
  SELECT * FROM [Movies] WHERE [Title] LIKE @param;
GO

-- Renamed updateRecord/updateMovie as Movie_Update
CREATE PROCEDURE [dbo].[Movie_Update]
  @pId int,
  @pTitle  [nvarchar](80),
  @pReleaseYear int,
  @pGenre  [nvarchar](100),
  @pDuration int
AS
  UPDATE [Movies] SET [Title] = @pTitle, [Genre] = @pGenre, [ReleaseYear] = @pReleaseYear, [Duration] = @pDuration WHERE [Id] = @pId;
GO

-- Movies.txt.txt: 41 rows
