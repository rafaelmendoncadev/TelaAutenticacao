CREATE TABLE [dbo].[Users] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(100) NOT NULL,
    [Password] NVARCHAR(100) NOT NULL
);

CREATE UNIQUE INDEX IX_Users_Email ON [dbo].[Users]([Email]);

INSERT INTO Users (Name, Email, Password) VALUES
('João da Silva', 'joao@email.com', '123456'),
('Maria Oliveira', 'maria@email.com', 'senha123'),
('Carlos Souza', 'carlos@email.com', 'abc123');

select * from Users
