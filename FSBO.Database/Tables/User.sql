CREATE TABLE [dbo].[User]
(
	[UserId] INT NOT NULL PRIMARY KEY IDENTITY,
	[Username] VARCHAR(32) NOT NULL,
	[Hash] VARBINARY(64) NOT NULL,
	[Salt] VARBINARY(128) NOT NULL,
    [Email] VARCHAR(254) NOT NULL,
	[Phone] VARCHAR(13) NOT NULL,
	[DayToCharge] TINYINT NOT NULL,
	[CurrentPaymentMethodId] INT,
    --[IsActive] BIT NOT NULL DEFAULT 0,
	CONSTRAINT [FK_User_CurrentPaymentMethodId] FOREIGN KEY ([CurrentPaymentMethodId]) REFERENCES [UserPaymentMethod]([UserPaymentMethodId])
)
