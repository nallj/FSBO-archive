CREATE TABLE [dbo].[SourceAreaEntry]
(
    [SourceId] INT NOT NULL,
    [AreaId] INT NOT NULL,
    [EntryPoint] VARCHAR(MAX) NOT NULL,
    [SourceUriOverride] NVARCHAR(1024) NULL,
    CONSTRAINT [PK_SourceAreaEntry] PRIMARY KEY ([SourceId], [AreaId]),
    CONSTRAINT [FK_SourceAreaEntry_SourceId] FOREIGN KEY ([SourceId]) REFERENCES [Source]([SourceId]) ON DELETE CASCADE,
    CONSTRAINT [FK_SourceAreaEntry_AreaId] FOREIGN KEY ([AreaId]) REFERENCES [Area]([AreaId]) ON DELETE CASCADE
)
--GO

--CREATE UNIQUE INDEX [UQ_SourceAreaEntryPoint_SourceIdAreaId] ON [dbo].[SourceAreaEntry] ([SourceId], [AreaId])