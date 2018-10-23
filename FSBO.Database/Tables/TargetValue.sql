CREATE TABLE [dbo].[TargetValue]
(
    [ValueId] INT NOT NULL PRIMARY KEY IDENTITY,
    [RecordId] INT NOT NULL,
    [TargetId] INT NOT NULL,
    [Value] NVARCHAR(MAX) NOT NULL,
    CONSTRAINT [FK_FieldValue_RecordId] FOREIGN KEY ([RecordId]) REFERENCES [ScrapeRecord] ([RecordId]),
    CONSTRAINT [FK_FieldValue_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [TargetField] ([TargetId])
)
