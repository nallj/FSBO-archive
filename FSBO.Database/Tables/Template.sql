CREATE TABLE [dbo].[Template]
(
    [TemplateId] INT NOT NULL PRIMARY KEY IDENTITY,
	[SourceId] INT NOT NULL,
    [Name] VARCHAR(128) NOT NULL,
	[IsTopLevel] BIT NOT NULL DEFAULT 0,
	CONSTRAINT [FK_Template_SourceId] FOREIGN KEY ([SourceId]) REFERENCES [Source]([SourceId])
)
