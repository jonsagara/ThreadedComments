CREATE TABLE [dbo].[Board]
(
[BoardId] [int] NOT NULL IDENTITY(1, 1),
[Name] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Board] ADD CONSTRAINT [PK_Board] PRIMARY KEY CLUSTERED  ([BoardId]) ON [PRIMARY]
GO
