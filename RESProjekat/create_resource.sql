CREATE TABLE [dbo].[resource]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [rname] VARCHAR(30) NULL, 
    [title] VARCHAR(30) NULL, 
    [type] INT NOT NULL, 
    [connectedTo] INT NULL, 
    [connectedType] INT NULL, 
    [description] VARCHAR(50) NULL, 
    CONSTRAINT [FK_resource_TypeTable] FOREIGN KEY ([type]) REFERENCES [TypeTable]([id])
)
