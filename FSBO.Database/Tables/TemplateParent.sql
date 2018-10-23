CREATE TABLE [dbo].[TemplateParent]
(
    [TemplateId] INT NOT NULL,
    [ParentId] INT NOT NULL,
    CONSTRAINT [PK_TemplateParent_SourceId] PRIMARY KEY ([TemplateId], [ParentId]),
    CONSTRAINT [FK_TemplateParent_TemplateId] FOREIGN KEY ([TemplateId]) REFERENCES [Template]([TemplateId]),
    CONSTRAINT [FK_TemplateParent_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [Template]([TemplateId])
)
