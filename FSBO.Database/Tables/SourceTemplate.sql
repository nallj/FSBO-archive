/*CREATE TABLE [dbo].[SourceTemplate]
(
    [SourceId] INT NOT NULL,
    [TemplateId] INT NOT NULL,
    CONSTRAINT [PK_SourceTemplate_SourceId] PRIMARY KEY ([SourceId], [TemplateId]),
    CONSTRAINT [FK_SourceTemplate_SourceId] FOREIGN KEY ([SourceId]) REFERENCES [Source]([SourceId]),
    CONSTRAINT [FK_SourceTemplate_TemplateId] FOREIGN KEY ([TemplateId]) REFERENCES [Template]([TemplateId])
)
GO

CREATE UNIQUE INDEX [UQ_SourceTemplate_TemplateId] ON [dbo].[SourceTemplate] ([TemplateId])*/