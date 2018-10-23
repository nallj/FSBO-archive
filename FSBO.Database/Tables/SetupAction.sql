CREATE TABLE [dbo].[SetupAction]
(
    [SetupActionId] INT NOT NULL PRIMARY KEY IDENTITY,
    [TemplateId] INT NOT NULL,
    [ActionTypeId] INT NOT NULL,
    [OrderIndex] INT NOT NULL,
    [Parameters] NVARCHAR(MAX),
    CONSTRAINT [FK_SetupAction_TemplateId] FOREIGN KEY ([TemplateId]) REFERENCES [Template]([TemplateId]),
    CONSTRAINT [FK_SetupAction_ActionTypeId] FOREIGN KEY ([ActionTypeId]) REFERENCES [ScrapeActionType]([ActionTypeId])
)
