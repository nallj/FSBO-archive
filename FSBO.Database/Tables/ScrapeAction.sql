CREATE TABLE [dbo].[ScrapeAction]
(
    [ActionId] INT NOT NULL PRIMARY KEY IDENTITY,
    [TemplateFieldId] INT,-- NOT NULL, -- This shouldn't really be allowed to be NULL, but I'm trying something so give me a break.
    [ActionTypeId] INT NOT NULL,
    [OrderIndex] INT NOT NULL,
    [Parameters] NVARCHAR(MAX),
    CONSTRAINT [FK_ScrapeAction_TemplateFieldId] FOREIGN KEY ([TemplateFieldId]) REFERENCES [TemplateField]([TemplateFieldId]),
    CONSTRAINT [FK_ScrapeAction_ActionTypeId] FOREIGN KEY ([ActionTypeId]) REFERENCES [ScrapeActionType]([ActionTypeId])
)
