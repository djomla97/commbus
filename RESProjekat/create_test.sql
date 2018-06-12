CREATE TABLE [dbo].[test]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [rname] VARCHAR(30) NULL, 
    [title] VARCHAR(30) NULL, 
    [connectedTo] INT NULL, 
    [connectedType] INT NULL, 
    [description] VARCHAR(50) NULL, 
)
