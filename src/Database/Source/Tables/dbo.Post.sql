CREATE TABLE [dbo].[Post]
(
[PostId] [int] NOT NULL IDENTITY(1, 1),
[BoardId] [int] NOT NULL,
[Title] [nvarchar] (1024) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Body] [nvarchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AuthorName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AuthorEmail] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PostUtcDate] [datetime2] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Post] ADD CONSTRAINT [PK_Post] PRIMARY KEY CLUSTERED  ([PostId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Post] ADD CONSTRAINT [FK_Post_BoardId__Board_BoardId] FOREIGN KEY ([BoardId]) REFERENCES [dbo].[Board] ([BoardId])
GO
