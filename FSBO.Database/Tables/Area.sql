CREATE TABLE [dbo].[Area]
(
	[AreaId] INT NOT NULL IDENTITY PRIMARY KEY,
	[AreaTypeId] INT NOT NULL,
	[Value] NVARCHAR(255) NOT NULL,
	[ApprovedOn] DATETIMEOFFSET,
	[UserSuggestionId] INT,
    CONSTRAINT [FK_Area_AreaTypeId] FOREIGN KEY (AreaTypeId) REFERENCES [AreaType]([AreaTypeId]),
    CONSTRAINT [FK_Area_UserSuggestionId] FOREIGN KEY (UserSuggestionId) REFERENCES [UserSuggestion]([UserSuggestionId])
)