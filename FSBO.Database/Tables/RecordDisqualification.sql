CREATE TABLE [dbo].[RecordDisqualification]
(
    [DisqualificationId] INT NOT NULL PRIMARY KEY IDENTITY,
    [TemplateId] INT NOT NULL,
    [DisqualificationTypeId] INT NOT NULL,
    [Parameters] NVARCHAR(MAX),
    CONSTRAINT [FK_RecordDisqualifier_TemplateId] FOREIGN KEY ([TemplateId]) REFERENCES [Template]([TemplateId])
)
