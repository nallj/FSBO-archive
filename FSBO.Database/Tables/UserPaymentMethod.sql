CREATE TABLE [dbo].[UserPaymentMethod]
(
	[UserPaymentMethodId] INT PRIMARY KEY IDENTITY,
	[UserId] INT NOT NULL,
	[Name] NVARCHAR(128),

	-- Need to look into PCI compliance...
	[NameOnCard] NVARCHAR(128) NOT NULL,
	[CreditCardNumber] CHAR(16) NOT NULL,
	[ExpMonth] TINYINT NOT NULL,
	[ExpYear] SMALLINT NOT NULL,
	CONSTRAINT [FK_UserPaymentMethod_UserId] FOREIGN KEY ([UserId]) REFERENCES [User]([UserId]) ON DELETE CASCADE
)
