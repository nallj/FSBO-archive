CREATE TABLE [dbo].[TargetField]
(
    [TargetId] INT NOT NULL PRIMARY KEY IDENTITY,
    [Name] VARCHAR(64) NOT NULL,
    [FieldTypeId] INT NOT NULL,
	[IsTemporaryField] BIT NOT NULL DEFAULT 0,
    CONSTRAINT [FK_TargetField_FieldTypeId] FOREIGN KEY ([FieldTypeId]) REFERENCES [TargetFieldType]([FieldTypeId])
)
