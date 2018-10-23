CREATE TABLE [dbo].[ScrapeEvent]
(
    [EventId] INT NOT NULL PRIMARY KEY IDENTITY,
    [AreaId] INT NOT NULL,
    [TemplateId] INT NOT NULL,
    [TimeStamp] DATETIMEOFFSET(7) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    CONSTRAINT [FK_ScrapeEvent_AreaId] FOREIGN KEY ([AreaId]) REFERENCES [Area]([AreaId]),
    CONSTRAINT [FK_ScrapeEvent_TemplateId] FOREIGN KEY ([TemplateId]) REFERENCES [Template]([TemplateId])
)
