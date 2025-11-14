USE GameShopApp
GO

INSERT INTO [Countries] (Name)
VALUES
('Croatia'),
('Bosnia And Hercegovina'),
('United States Of America')

INSERT INTO [Cities] (Name,PostalCode,Latitude,Longitude,CountryId)
VALUES
('Zagreb','10000',45.8071 ,15.9644,1),
('Split','21000',43.508133 ,16.440193,1),
('Sarajevo','71000',43.8486 ,18.3564,2),
('Mostar','88000',43.343033 ,17.807894,2),
('New York','10001',40.730610 ,-73.935242,3),
('Los Angeles','90001',34.0522351 ,-118.243683,3)


INSERT INTO [Users] (UserName, PasswordHash, Email, PhoneNumber, FirstName, LastName, ProfileImageURL, CityId, CreationDate)
VALUES
('TooFatTooCarry','PasswordHash1','bojankozina@outlook.com','+3850923822446','Bojan','Kozina',NULL,1,GETDATE()),
('mp4321','PasswordHash2','malikpeco@gmail.com','+3870624655443','Malik','Peco',NULL,4,GETDATE()),
('thedejvid','PasswordHash3','someemail@outlook.com','+3850954822446','David','Kartinski',NULL,2,GETDATE()),
('Cooldudz','PasswordHash4','andrewandrew@yahoo.com','+0010563822446','Andrew','Andrews',NULL,6,GETDATE()),
('Test','Test','test@test.com','+3850923822446','Test','Test',NULL,3,GETDATE()),
('Parzival ','PasswordHash6','RealWade@outlook.com','+3850923822446','Wade','Owen Watts',NULL,5,GETDATE())

INSERT INTO [Publishers] (Name,CountryId)
VALUES
('CroTeam',1),
('BiHTeam',2),
('DICE',3)
('Ubisoft',3),

INSERT INTO [Games] 
(Name, Price, Description, PublisherId, ReleaseDate, CoverArtImageURL)
VALUES
(
  'Ramo: Journey through Sarajevo',
  5,
  'Exciting 2D platformer adventure set in the spring of 1992 in Sarajevo. 
   It provides players with a nostalgic journey through a turbulent time, 
   as the main character, Ramo, tries to fight off numerous enemies on his 
   way through the city streets.',
  2,
  '2024-3-10',
  'URL'
),
(
  'The Talos Principle Reawakened',
  39.99,
  'Step back into a world of thought-provoking puzzles, philosophical intrigue, 
   and breathtaking visuals with The Talos Principle: Reawakened - 
   a first-person puzzle masterpiece for both new players and long-time fans alike.',
  1,
  '2025-4-10',
  'URL'
),
(
  'Battlefield 1',
  39.99,
  'Battlefield™ 1 takes you back to The Great War, WW1, 
   where new technology and worldwide conflict changed the face of warfare forever.',
  1,
  '2020-10-20',
  'URL'
)

INSERT INTO [UserGames] (UserId,GameId,PurchaseDate)
VALUES
(1,4,'2025-10-25'),
(1,2,'2025-10-25'),
(1,3,'2025-10-25')

SELECT *
FROM Users
GO

SELECT *
FROM Cities
GO

SELECT* 
FROM Countries

SELECT*
FROM Publishers

SELECT*
FROM Games
GO

SELECT *
FROM UserGames
GO