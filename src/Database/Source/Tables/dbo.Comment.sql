CREATE TABLE [dbo].[Comment]
(
[CommentId] [int] NOT NULL IDENTITY(1, 1),
[PostId] [int] NOT NULL,
[BoardId] [int] NOT NULL,
[ParentCommentId] [int] NULL,
[Body] [nvarchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AuthorName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AuthorEmail] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CommentUtcDate] [datetime2] NOT NULL,
[SortOrder] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Indent] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Comment] ADD CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED  ([CommentId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Comment] ADD CONSTRAINT [FK_Comment_BoardId__Board_BoardId] FOREIGN KEY ([BoardId]) REFERENCES [dbo].[Board] ([BoardId])
GO
ALTER TABLE [dbo].[Comment] ADD CONSTRAINT [FK_Comment_CommentId__Comment_ParentCommentId] FOREIGN KEY ([ParentCommentId]) REFERENCES [dbo].[Comment] ([CommentId])
GO
ALTER TABLE [dbo].[Comment] ADD CONSTRAINT [FK_Comment_PostId__Post_PostId] FOREIGN KEY ([PostId]) REFERENCES [dbo].[Post] ([PostId])
GO
