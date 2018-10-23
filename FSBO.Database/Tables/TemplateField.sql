CREATE TABLE [dbo].[TemplateField]
(
    [TemplateFieldId] INT NOT NULL PRIMARY KEY IDENTITY,
    [TemplateId] INT NOT NULL,
    [TargetId] INT NOT NULL,
	[OrderIndex] INT NOT NULL
    CONSTRAINT [FK_TemplateField_TemplateId] FOREIGN KEY ([TemplateId]) REFERENCES [Template]([TemplateId]),
    CONSTRAINT [FK_TemplateField_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [TargetField]([TargetId])
)
