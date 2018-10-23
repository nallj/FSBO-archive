CREATE TABLE [dbo].[Subscriber]
(
    [SubscriberId] INT NOT NULL PRIMARY KEY IDENTITY,
    [Name] NVARCHAR(128) NOT NULL,
    [Email] VARCHAR(254) NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT 0
)
