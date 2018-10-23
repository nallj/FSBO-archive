CREATE TABLE [dbo].[ScrapeRecord]
(
    [RecordId] INT NOT NULL PRIMARY KEY IDENTITY,
    [EventId] INT NOT NULL,
    CONSTRAINT [FK_FieldValue_EventId] FOREIGN KEY ([EventId]) REFERENCES [ScrapeEvent] ([EventId])
)
