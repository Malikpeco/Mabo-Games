--USE master
--GO
--DROP DATABASE GameShopApp

--CREATE DATABASE GameShopApp
--GO
--USE GameShopApp

CREATE TABLE [Users] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [UserName] nvarchar(20) UNIQUE NOT NULL,
  [PasswordHash] nvarchar(250) NOT NULL,
  [Email] nvarchar(50) UNIQUE NOT NULL,
  [PhoneNumber] nvarchar(20),
  [FirstName] nvarchar(50) NOT NULL,
  [LastName] nvarchar(50) NOT NULL,
  [ProfileImageURL] nvarchar(255),
  [CityId] int NOT NULL,
  [CreationDate] dateTime NOT NULL
)
GO

CREATE TABLE [Games] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Name] nvarchar(100) UNIQUE NOT NULL,
  [Price] money NOT NULL,
  [Description] text,
  [ReleaseDate] date NOT NULL,
  [CoverArt] nvarchar (255) NOT NULL
  [PublisherId] int NOT NULL,
)
GO

CREATE TABLE [UserGames] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [UserId] int NOT NULL,
  [GameId] int NOT NULL,
  [PurchaseDate] datetime NOT NULL
)
GO

CREATE TABLE [Cities] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Name] nvarchar(100) UNIQUE NOT NULL,
  [Latitude] float NOT NULL,
  [Longitude] float NOT NULL,
  [PostalCode] nvarchar(20) NOT NULL,
  [CountryId] int NOT NULL
)
GO

CREATE TABLE [Countries] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Name] nvarchar(100) UNIQUE NOT NULL
)
GO

CREATE TABLE [Publishers] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Name] nvarchar(100) UNIQUE NOT NULL,
  [CountryId] int NOT NULL
)
GO

CREATE TABLE [Genres] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Name] nvarchar(50) UNIQUE NOT NULL
)
GO

CREATE TABLE [GamesGenres] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [GameId] int NOT NULL,
  [GenreId] int NOT NULL
)
GO

CREATE TABLE [Screenshots] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [ImageURL] nvarchar(255),
  [GameId] int NOT NULL
)
GO

CREATE TABLE [Reviews] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Content] text,
  [Rating] float NOT NULL,
  [ReviewDate] datetime NOT NULL,
  [UserId] int NOT NULL,
  [GameId] int NOT NULL
)
GO

CREATE TABLE [Achievements] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Name] nvarchar(50) UNIQUE NOT NULL,
  [Description] nvarchar(100),
  [IconImageURL] nvarchar(255) NOT NULL,
  [GameId] int
)
GO

CREATE TABLE [UserAchievements] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [UserId] int NOT NULL,
  [AchievementId] int NOT NULL,
  [AchievedAt] dateTime NOT NULL
)
GO

CREATE TABLE [Notification] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Title] nvarchar(100) NOT NULL,
  [Content] nvarchar(200) NOT NULL,
  [IsRead] bit NOT NULL DEFAULT (0),
  [SentAt] dateTIME NOT NULL,
  [UserId] int NOT NULL
)
GO

CREATE TABLE [Favourites] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [FavouritedAt] date NOT NULL,
  [UserId] int NOT NULL,
  [GameId] int NOT NULL
)
GO

CREATE TABLE [Orders] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [OrderDate] datetime NOT NULL,
  [TotalAmount] money NOT NULL,
  [PaymentStatus] nvarchar(255) NOT NULL CHECK ([PaymentStatus] IN ('Successful', 'Pending', 'Failed')),
  [UserId] int NOT NULL
)
GO

CREATE TABLE [OrderItems] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Price] money NOT NULL,
  [OrderId] int NOT NULL,
  [GameId] int NOT NULL
)
GO

CREATE TABLE [Payments] (
  [Id] int PRIMARY KEY,
  [OrderId] int NOT NULL,
  [StripeTransactionId] nvarchar(250) NOT NULL,
  [Total] money NOT NULL,
  [PaymentDate] datetime NOT NULL
)
GO

CREATE TABLE [CartItems] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [UserId] int NOT NULL,
  [GameId] int NOT NULL,
  [AddedAt] datetime NOT NULL
)
GO

CREATE TABLE [AuditLog] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [UserId] int NOT NULL,
  [Action] nvarchar(100) NOT NULL,
  [EntityName] nvarchar(100) NOT NULL,
  [EntityId] int NOT NULL,
  [Timestamp] datetime NOT NULL,
  [Description] text
)
GO

CREATE TABLE [SecurityQuestions] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [SecurityQuestion] nvarchar(200) UNIQUE NOT NULL
)
GO

CREATE TABLE [UserSecurityQuestions] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [UserId] int NOT NULL,
  [SecurityQuestionId] int NOT NULL,
  [AnswerHash] nvarchar(250) NOT NULL
)
GO

CREATE UNIQUE INDEX [UserGames_index_0] ON [UserGames] ("UserId", "GameId")
GO

CREATE UNIQUE INDEX [CartItems_index_1] ON [CartItems] ("UserId", "GameId")
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Napravti restriction za minimalni lenght!',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Users',
@level2type = N'Column', @level2name = 'UserName';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Napravti restriction za minimalni lenght!',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Users',
@level2type = N'Column', @level2name = 'PasswordHash';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Napraviti check da email izgleda kao ''%@%.%''',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Users',
@level2type = N'Column', @level2name = 'Email';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Stavit defualt kao GETDATE()',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'UserGames',
@level2type = N'Column', @level2name = 'PurchaseDate';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Napraviti restriction da je izmedu 0 i 5',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Reviews',
@level2type = N'Column', @level2name = 'Rating';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Staviti defualt kao DATETODAY()',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Reviews',
@level2type = N'Column', @level2name = 'ReviewDate';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Staviti defualt kao DATETODAY()',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'UserAchievements',
@level2type = N'Column', @level2name = 'AchievedAt';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Bit ce vrijeme kad je poslan notification',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Notification',
@level2type = N'Column', @level2name = 'SentAt';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Staviti defualt kao DATETODAY()',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Favourites',
@level2type = N'Column', @level2name = 'FavouritedAt';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Staviti defualt kao DATETODAY()',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Orders',
@level2type = N'Column', @level2name = 'OrderDate';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Mozda se zove token ali nije bitno',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'Payments',
@level2type = N'Column', @level2name = 'StripeTransactionId';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Default bi trebao biti GETDATE()',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'AuditLog',
@level2type = N'Column', @level2name = 'Timestamp';
GO

ALTER TABLE [GamesGenres] ADD FOREIGN KEY ([GameId]) REFERENCES [Games] ([Id])
GO

ALTER TABLE [GamesGenres] ADD FOREIGN KEY ([GenreId]) REFERENCES [Genres] ([Id])
GO

ALTER TABLE [Games] ADD FOREIGN KEY ([PublisherId]) REFERENCES [Publishers] ([Id])
GO

ALTER TABLE [Users] ADD FOREIGN KEY ([CityId]) REFERENCES [Cities] ([Id])
GO

ALTER TABLE [Cities] ADD FOREIGN KEY ([CountryId]) REFERENCES [Countries] ([Id])
GO

ALTER TABLE [Publishers] ADD FOREIGN KEY ([CountryId]) REFERENCES [Countries] ([Id])
GO

ALTER TABLE [Screenshots] ADD FOREIGN KEY ([GameId]) REFERENCES [Games] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [Reviews] ADD FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [Reviews] ADD FOREIGN KEY ([GameId]) REFERENCES [Games] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [Achievements] ADD FOREIGN KEY ([GameId]) REFERENCES [Games] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [UserAchievements] ADD FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [UserAchievements] ADD FOREIGN KEY ([AchievementId]) REFERENCES [Achievements] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [Notification] ADD FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [Favourites] ADD FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [Favourites] ADD FOREIGN KEY ([GameId]) REFERENCES [Games] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [Orders] ADD FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
GO

ALTER TABLE [OrderItems] ADD FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [OrderItems] ADD FOREIGN KEY ([GameId]) REFERENCES [Games] ([Id])
GO

ALTER TABLE [CartItems] ADD FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [CartItems] ADD FOREIGN KEY ([GameId]) REFERENCES [Games] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [Payments] ADD FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id])
GO

ALTER TABLE [UserGames] ADD FOREIGN KEY ([GameId]) REFERENCES [Games] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [UserGames] ADD FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [AuditLog] ADD FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
GO

ALTER TABLE [UserSecurityQuestions] ADD FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [UserSecurityQuestions] ADD FOREIGN KEY ([SecurityQuestionId]) REFERENCES [SecurityQuestions] ([Id])
GO
