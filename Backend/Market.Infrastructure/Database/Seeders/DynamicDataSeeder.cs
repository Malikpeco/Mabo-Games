using Market.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using static System.Net.WebRequestMethods;

namespace Market.Infrastructure.Database.Seeders;

public static class DynamicDataSeeder
{
    public static async Task SeedAsync(DatabaseContext context)
    {
        // Ensure DB is created (no migrations)
        await context.Database.EnsureCreatedAsync();

        await SeedCountriesAsync(context);
        await SeedUsersAsync(context);
        await SeedReviewUsersAsync(context);
        await SeedSecurityQuestionsAsync(context);
        await SeedPublishersAsync(context);
        await SeedGenresAsync(context);
        await SeedGamesAsync(context);
        await SeedGameGenresAsync(context);
        await SeedGameReviewsAsync(context);
        await SeedIGDBToken(context);
        await SeedAchievementsAsync(context);
    }

    private static async Task SeedIGDBToken(DatabaseContext context)
    {
        if (await context.IGDBTokens.AnyAsync())
            return;

        var token = new IGDBTokenEntity
        {

            Token = "202bz70ncku4b8lrviwryp31fvmhta",
            ExpiresAt = DateTime.ParseExact("2026-04-26 21:05:44.4339604", "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture),
            IsDeleted = false,
            LastUpdated = DateTime.ParseExact("2026-03-01 21:11:59.4340855", "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture),
            CreatedAtUtc = DateTime.ParseExact("2026-03-01 21:11:59.4427116", "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture),
            ModifiedAtUtc = null,


        };

        context.IGDBTokens.Add(token);
        await context.SaveChangesAsync();

        Console.WriteLine($"Dynamic seed: IGDB Token added.");



    }
    private static async Task SeedCountriesAsync(DatabaseContext context)
    {

        if (await context.Countries.AnyAsync())
            return;




        var countries = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            .Select(c => new RegionInfo(c.Name))
            .Select(r => new CountryEntity
            {
                Name = r.EnglishName,
            })
            .GroupBy(c => c.Name)
            .Select(g => g.First())
            .ToList();

        context.Countries.AddRange(countries);
        await context.SaveChangesAsync();

        Console.WriteLine($"Dynamic seed: {countries.Count} Countries added.");
    }

    private static async Task SeedSecurityQuestionsAsync(DatabaseContext context)
    {
        if (await context.SecurityQuestions.AnyAsync())
            return;

        var questionOne = new SecurityQuestionEntity
        {
            Question = "What was the name of your first pet?"
        };

        var questionTwo = new SecurityQuestionEntity
        {
            Question = "What was your childhood nickname?"
        };

        var questionThree = new SecurityQuestionEntity
        {
            Question = "In what city were you born?"

        };


        context.SecurityQuestions.AddRange(questionOne, questionTwo, questionThree);
        await context.SaveChangesAsync();

        Console.WriteLine("Dynamic seed: Security Questions added.");
    }

    private static async Task SeedAchievementsAsync(DatabaseContext context)
    {
        if (await context.Achievements.AnyAsync())
            return;

        var ach1 = new AchievementEntity
        {
           Name="First Blood",
           Description="Purchase your first game",
           ImageURL= "https://i.ibb.co/3ywP4pdb/Icons-21.png"

        };

        var ach2 = new AchievementEntity
        {
            Name = "Collector",
            Description = "Own 10 games",
            ImageURL = "https://i.ibb.co/C5mNs4D5/Icons-23.png"

        };

        var ach3 = new AchievementEntity
        {
            Name = "Hoarder",
            Description = "Own 100 games",
            ImageURL = "https://i.ibb.co/cnCGmtq/Icons-24.png"

        };

        var ach4 = new AchievementEntity
        {
            Name = "I know my taste",
            Description = "Write a review for a game",
            ImageURL = "https://i.ibb.co/RTdNZ4Gr/Icons-48.png"

        };


        var ach5 = new AchievementEntity
        {
            Name = "Critic",
            Description = "Write 10 reviews",
            ImageURL = "https://i.ibb.co/395wz3jR/Icons-50.png"

        };

        var ach6 = new AchievementEntity
        {
            Name = "Genre explorer",
            Description = "Buy games from 5 different genres",
            ImageURL = "https://i.ibb.co/SXY9L9Hk/Icons-11.png"

        };

        var ach7 = new AchievementEntity
        {
            Name = "Fanboy",
            Description = "Own 3 games from the same publisher",
            ImageURL = "https://i.ibb.co/4nyL9zfz/Icons-25.png"

        };

        var ach8 = new AchievementEntity
        {
            Name = "Night Owl",
            Description = "Purchase a game after midnight",
            ImageURL = "https://i.ibb.co/XZdCfb9k/Icons-20.png"

        };






        context.Achievements.AddRange(ach1,ach2,ach3,ach4, ach5, ach6, ach7, ach8);
        await context.SaveChangesAsync();

        Console.WriteLine("Dynamic seed: Security Questions added.");
    }




    private static async Task SeedUsersAsync(DatabaseContext context)
    {
        if (await context.Users.AnyAsync())
            return;

        var hasher = new PasswordHasher<UserEntity>();

        var defaultCountry = await context.Countries.FirstAsync();


        var admin = new UserEntity
        {
            Username = "Admin",
            Email = "admin@market.com",
            FirstName = "Admin",
            LastName = "Admin",
            IsAdmin = true,
            IsEnabled = true,
            CreationDate = DateTime.UtcNow,
        };

        admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");


        var user = new UserEntity
        {
            Username = "User",
            Email = "user@market.com",
            FirstName = "User",
            LastName = "User",
            IsAdmin = false,
            IsEnabled = true,
            CreationDate = DateTime.UtcNow,
        };

        user.PasswordHash = hasher.HashPassword(user, "User123!");



        // Assign carts (required)
        admin.Cart = new CartEntity { User = admin };
        user.Cart = new CartEntity { User = user };

        context.Users.AddRange(admin, user);

        await context.SaveChangesAsync();

        Console.WriteLine("Dynamic seed: demo users added.");
    }

    private static async Task SeedReviewUsersAsync(DatabaseContext context)
    {
        if (await context.Users.AnyAsync(u => u.Username.StartsWith("Reviewer")))
            return;

        var hasher = new PasswordHasher<UserEntity>();
        var reviewers = new List<UserEntity>();

        for (int i = 1; i <= 5; i++)
        {
            var reviewer = new UserEntity
            {
                Username = $"Reviewer{i}",
                Email = $"reviewer{i}@market.com",
                FirstName = "Reviewer",
                LastName = $"{i}",
                IsAdmin = false,
                IsEnabled = true,
                CreationDate = DateTime.UtcNow,
            };

            reviewer.PasswordHash = hasher.HashPassword(reviewer, "Reviewer123!");
            reviewer.Cart = new CartEntity { User = reviewer };

            reviewers.Add(reviewer);
        }

        context.Users.AddRange(reviewers);
        await context.SaveChangesAsync();

        Console.WriteLine("Dynamic seed: Reviewer users added.");
    }

    private static async Task SeedPublishersAsync(DatabaseContext context)
    {
        if (await context.Publishers.AnyAsync())
            return;

        var pub1 = new PublisherEntity
        {
            Name = "Rockstar Games",
            CountryId = 52
        };

        var pub2 = new PublisherEntity
        {
            Name = "EA",
            CountryId = 52
        };

        var pub3 = new PublisherEntity
        {
            Name = "CD Projekt Red",
            CountryId = 228
        };

        var pub4 = new PublisherEntity
        {
            Name = "FromSoftware",
            CountryId = 204
        };

        var pub5 = new PublisherEntity
        {
            Name = "Sony Interactive Entertainment",
            CountryId = 204
        };

        var pub6 = new PublisherEntity
        {
            Name = "Supergiant Games",
            CountryId = 52
        };

        var pub7 = new PublisherEntity
        {
            Name = "EA Sports",
            CountryId = 52
        };

        var pub8 = new PublisherEntity
        {
            Name = "BosnianKingdom",
            CountryId = 47
        };

        var pub9 = new PublisherEntity
        {
            Name = "Ubisoft",
            CountryId = 46
        };

        var pub10 = new PublisherEntity
        {
            Name = "Capcom",
            CountryId = 204
        };

        var pub11 = new PublisherEntity
        {
            Name = "Bethesda Softworks",
            CountryId = 52
        };

        var pub12 = new PublisherEntity
        {
            Name = "Valve",
            CountryId = 52
        };

        var pub13 = new PublisherEntity
        {
            Name = "Square Enix",
            CountryId = 204
        };

        var pub14 = new PublisherEntity
        {
            Name = "Konami",
            CountryId = 204
        };

        var pub15 = new PublisherEntity
        {
            Name = "Poncle",
            CountryId = 52
        };

        var pub16 = new PublisherEntity
        {
            Name = "Innersloth",
            CountryId = 52
        };

        var pub17 = new PublisherEntity
        {
            Name = "PopCap Games",
            CountryId = 52
        };

        var pub18 = new PublisherEntity
        {
            Name = "Playsaurus",
            CountryId = 52
        };

        var pub19 = new PublisherEntity
        {
            Name = "Erabit Studios",
            CountryId = 52
        };

        var pub20 = new PublisherEntity
        {
            Name = "Distractionware",
            CountryId = 52
        };

        var pub21 = new PublisherEntity
        {
            Name = "Silver Dollar Games",
            CountryId = 52
        };

        var pub22 = new PublisherEntity
        {
            Name = "Devolver Digital",
            CountryId = 52
        };

        var pub23 = new PublisherEntity
        {
            Name = "Matthew Brown Games",
            CountryId = 52
        };

        var pub24 = new PublisherEntity
        {
            Name = "Digital Extremes",
            CountryId = 52
        };

        var pub25 = new PublisherEntity
        {
            Name = "Smartly Dressed Games",
            CountryId = 52
        };

        var pub26 = new PublisherEntity
        {
            Name = "NetEase",
            CountryId = 52
        };

        context.Publishers.AddRange(pub1, pub2, pub3, pub4, pub5, pub6, pub7, pub8, pub9, pub10, pub11, pub12, pub13, pub14,
            pub15, pub16, pub17, pub18, pub19, pub20, pub21, pub22, pub23, pub24, pub25, pub26);
        await context.SaveChangesAsync();

        Console.WriteLine("Dynamic seed: Publishers added.");
    }
    private static async Task SeedGenresAsync(DatabaseContext context)
    {
        if (await context.Genres.AnyAsync())
            return;

        var g1 = new GenreEntity
        {
            Name = "Action",
            
        };

        var g2 = new GenreEntity
        {
            Name = "Role-Playing (RPG)",
        };

        var g3 = new GenreEntity
        {
            Name = "Adventure",
        };

        var g4 = new GenreEntity
        {
            Name = "Sports",
        };

        var g5 = new GenreEntity
        {
            Name = "Strategy",
        };

        var g6 = new GenreEntity
        {
            Name = "Open-World",
        };

        var g7 = new GenreEntity
        {
            Name = "Survival",
        };

        var g8 = new GenreEntity
        {
            Name = "Horror",
        };

        var g9 = new GenreEntity
        {
            Name = "Racing",
        };

        var g10 = new GenreEntity
        {
            Name = "Puzzle",
        };

        var g11 = new GenreEntity
        {
            Name = "Simulation",
        };

        context.Genres.AddRange(g1, g2, g3, g4, g5, g6, g7, g8, g9, g10, g11);
        await context.SaveChangesAsync();

        Console.WriteLine("Dynamic seed: Genres added.");
    }

    private static async Task SeedGamesAsync(DatabaseContext context)
    {
        if (await context.Games.AnyAsync())
            return;

        var gm1 = new GameEntity
        {
            Name = "Grand Theft Auto 'San Andreas'",
            PublisherId = 1,
            Price = 59.99m,
            ReleaseDate = new DateTime(2013, 9, 17),
            Description = "An open-world action-adventure game following three criminals in the fictional state of San Andreas.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2lb9.jpg",
            GameFilePath = "gameFile1.bin"
        };

        var gm2 = new GameEntity
        {
            Name = "FIFA 19",
            PublisherId = 2,
            Price = 59.99m,
            ReleaseDate = new DateTime(2018, 9, 28),
            Description = "A football simulation game featuring realistic gameplay, official leagues and teams, and the conclusion of The Journey story mode.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co68bt.jpg",
            GameFilePath = "gameFile2.bin"
        };

        var gm3 = new GameEntity
        {
            Name = "Red Dead Redemption 2",
            PublisherId = 1,
            Price = 59.99m,
            ReleaseDate = new DateTime(2018, 10, 26),
            Description = "An epic tale of outlaw Arthur Morgan and the Van der Linde gang in the dying days of the Wild West.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co1q1f.jpg",
            GameFilePath = "gameFile3.bin"
        };

        var gm4 = new GameEntity
        {
            Name = "The Witcher 3: Wild Hunt",
            PublisherId = 3,
            Price = 39.99m,
            ReleaseDate = new DateTime(2015, 5, 19),
            Description = "A story-driven open world RPG set in a visually stunning fantasy universe full of meaningful choices.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/coaarl.jpg",
            GameFilePath = "gameFile4.bin"
        };

        var gm5 = new GameEntity
        {
            Name = "Cyberpunk 2077",
            PublisherId = 3,
            Price = 59.99m,
            ReleaseDate = new DateTime(2020, 12, 10),
            Description = "An open-world action-adventure story set in Night City, a megalopolis obsessed with power and glamour.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/coaih8.jpg",
            GameFilePath = "gameFile5.bin"
        };

        var gm6 = new GameEntity
        {
            Name = "Elden Ring",
            PublisherId = 4,
            Price = 59.99m,
            ReleaseDate = new DateTime(2022, 2, 25),
            Description = "A vast action RPG world filled with mystery and danger, created by Hidetaka Miyazaki and George R. R. Martin.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co4jni.jpg",
            GameFilePath = "gameFile6.bin"
        };

        var gm7 = new GameEntity
        {
            Name = "Hades",
            PublisherId = 6,
            Price = 24.99m,
            ReleaseDate = new DateTime(2020, 9, 17),
            Description = "A rogue-like dungeon crawler where you defy the god of the dead while wielding mythic weapons.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/cob9kr.jpg",
            GameFilePath = "gameFile7.bin"
        };



        var gm9 = new GameEntity
        {
            Name = "Tomb Raider",
            PublisherId = 1,
            Price = 19.99m,
            ReleaseDate = new DateTime(2026, 1, 2),
            Description = "this is a test game boi",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co1rbu.jpg",
            GameFilePath = "gameFile8.bin"
        };

        var gm10 = new GameEntity
        {
            Name = "God of War",
            PublisherId = 5,
            Price = 49.99m,
            ReleaseDate = new DateTime(2018, 4, 20),
            Description = "A mythological action-adventure following Kratos in Norse lands.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/cobkt6.jpg",
            GameFilePath = "gameFile9"
        };

        var gm11 = new GameEntity
        {
            Name = "God of War Ragnarök",
            PublisherId = 5,
            Price = 59.99m,
            ReleaseDate = new DateTime(2022, 11, 9),
            Description = "The epic continuation of Kratos and Atreus’ Norse saga.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/coba3d.jpg",
            GameFilePath = "gameFile10.bin"
        };

        var gm12 = new GameEntity
        {
            Name = "Assassin's Creed Valhalla",
            PublisherId = 8,
            Price = 59.99m,
            ReleaseDate = new DateTime(2020, 11, 10),
            Description = "An open-world Viking adventure set in Dark Ages England.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2ed3.jpg",
            GameFilePath = "gameFile11.bin"
        };

        var gm13 = new GameEntity
        {
            Name = "Assassin's Creed Odyssey",
            PublisherId = 8,
            Price = 39.99m,
            ReleaseDate = new DateTime(2018, 10, 5),
            Description = "Explore ancient Greece in this vast open-world RPG.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2nul.jpg",
            GameFilePath = "gameFile12.bin"
        };

        var gm14 = new GameEntity
        {
            Name = "Resident Evil 4 Remake",
            PublisherId = 9,
            Price = 59.99m,
            ReleaseDate = new DateTime(2023, 3, 24),
            Description = "A modern reimagining of the legendary survival horror game.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co6bo0.jpg",
            GameFilePath = "gameFile13.bin"
        };

        var gm15 = new GameEntity
        {
            Name = "Resident Evil Village",
            PublisherId = 9,
            Price = 39.99m,
            ReleaseDate = new DateTime(2021, 5, 7),
            Description = "Survival horror set in a mysterious European village.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/coab9q.jpg",
            GameFilePath = "gameFile14.bin"
        };

        var gm16 = new GameEntity
        {
            Name = "Dark Souls III",
            PublisherId = 4,
            Price = 59.99m,
            ReleaseDate = new DateTime(2016, 4, 12),
            Description = "A challenging action RPG set in a dark fantasy world.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/cob9ed.jpg",
            GameFilePath = "gameFile15.bin"
        };

        var gm17 = new GameEntity
        {
            Name = "Sekiro: Shadows Die Twice",
            PublisherId = 4,
            Price = 59.99m,
            ReleaseDate = new DateTime(2019, 3, 22),
            Description = "A precision-based action game set in feudal Japan.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2a23.jpg",
            GameFilePath = "gameFile16.bin"
        };

        var gm18 = new GameEntity
        {
            Name = "Starfield",
            PublisherId = 10,
            Price = 69.99m,
            ReleaseDate = new DateTime(2023, 9, 6),
            Description = "A massive space RPG exploring the vastness of the universe.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co39vv.jpg",
            GameFilePath = "gameFile17.bin"
        };

        var gm19 = new GameEntity
        {
            Name = "The Elder Scrolls V: Skyrim",
            PublisherId = 10,
            Price = 39.99m,
            ReleaseDate = new DateTime(2011, 11, 11),
            Description = "An open-world fantasy RPG set in the land of Skyrim.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co1tnw.jpg",
            GameFilePath = "gameFile18.bin"
        };

        var gm20 = new GameEntity
        {
            Name = "Fallout 4",
            PublisherId = 10,
            Price = 19.99m,
            ReleaseDate = new DateTime(2015, 11, 10),
            Description = "A post-apocalyptic RPG set in the ruins of Boston.",
            CoverImageURL = "https://image.api.playstation.com/vulcan/ap/rnd/202009/2502/rB3GRFvdPmaALiGt89ysflQ4.jpg",
            GameFilePath = "gameFile19.bin"
        };

        var gm21 = new GameEntity
        {
            Name = "DOOM Eternal",
            PublisherId = 10,
            Price = 29.99m,
            ReleaseDate = new DateTime(2020, 3, 20),
            Description = "Fast-paced demon-slaying FPS action.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co1yc6.jpg",
            GameFilePath = "gameFile20.bin"
        };

        var gm22 = new GameEntity
        {
            Name = "Half-Life: Alyx",
            PublisherId = 11,
            Price = 59.99m,
            ReleaseDate = new DateTime(2020, 3, 23),
            Description = "A VR return to the Half-Life universe.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co87vg.jpg",
            GameFilePath = "gameFile21.bin"
        };

        var gm23 = new GameEntity
        {
            Name = "Horizon Zero Dawn",
            PublisherId = 5,
            Price = 19.99m,
            ReleaseDate = new DateTime(2017, 2, 28),
            Description = "An open-world action RPG in a post-apocalyptic world.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2una.jpg",
            GameFilePath = "gameFile22.bin"
        };

        var gm24 = new GameEntity
        {
            Name = "Horizon Forbidden West",
            PublisherId = 5,
            Price = 59.99m,
            ReleaseDate = new DateTime(2022, 2, 18),
            Description = "The continuation of Aloy’s journey in a dangerous frontier.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2gvu.jpg",
            GameFilePath = "gameFile23.bin"
        };

        var gm25 = new GameEntity
        {
            Name = "Metal Gear Solid V: The Phantom Pain",
            PublisherId = 13,
            Price = 19.99m,
            ReleaseDate = new DateTime(2015, 9, 1),
            Description = "A tactical stealth game with an open-world design.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co1v85.jpg",
            GameFilePath = "gameFile24.bin"
        };

        var gm26 = new GameEntity
        {
            Name = "Battlefield 1",
            PublisherId = 2,
            Price = 19.99m,
            ReleaseDate = new DateTime(2016, 10, 21),
            Description = "A World War I themed first-person shooter.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2n9d.jpg",
            GameFilePath = "gameFile25.bin"
        };

        var gm27 = new GameEntity
        {
            Name = "Battlefield V",
            PublisherId = 2,
            Price = 19.99m,
            ReleaseDate = new DateTime(2018, 11, 20),
            Description = "A WWII shooter focused on large-scale battles.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co1xbv.jpg",
            GameFilePath = "gameFile26.bin"
        };

        var gm28 = new GameEntity
        {
            Name = "Death Stranding",
            PublisherId = 5,
            Price = 39.99m,
            ReleaseDate = new DateTime(2019, 11, 8),
            Description = "A unique narrative-driven experience in a fractured world.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/cobksf.jpg",
            GameFilePath = "gameFile27.bin"
        };

        var gm29 = new GameEntity
        {
            Name = "Team Fortress 2",
            PublisherId = 12,
            Price = 0.00m,
            ReleaseDate = new DateTime(2007, 10, 10),
            Description = "A team-based multiplayer shooter with nine distinct mercenary classes.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co6rzl.jpg",
            GameFilePath = "gameFile28.bin"
        };

        var gm30 = new GameEntity
        {
            Name = "Vampire Survivors",
            PublisherId = 15,
            Price = 4.99m,
            ReleaseDate = new DateTime(2022, 10, 20),
            Description = "A gothic horror roguelike where minimalist gameplay meets a rich world.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co4bzv.jpg",
            GameFilePath = "gameFile29.bin"
        };

        var gm31 = new GameEntity
        {
            Name = "Among Us",
            PublisherId = 16,
            Price = 2.99m,
            ReleaseDate = new DateTime(2018, 11, 16),
            Description = "An online and local party game of teamwork and betrayal for 4-15 players in space.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co3k9v.jpg",
            GameFilePath = "gameFile30.bin"
        };

        var gm32 = new GameEntity
        {
            Name = "Plants vs. Zombies: Game of the Year Edition",
            PublisherId = 17,
            Price = 4.99m,
            ReleaseDate = new DateTime(2011, 5, 26),
            Description = "A tower-defense classic where zombies are hungry for brains and only your plants can stop them.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co9n56.jpg",
            GameFilePath = "gameFile31.bin"
        };

        var gm33 = new GameEntity
        {
            Name = "Peggle Deluxe",
            PublisherId = 17,
            Price = 4.99m,
            ReleaseDate = new DateTime(2007, 3, 6),
            Description = "55 fanciful levels with 10 mystical Magic Powers in a physics-based puzzle game.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2h9x.jpg",
            GameFilePath = "gameFile32.bin"
        };

        var gm34 = new GameEntity
        {
            Name = "Cookie Clicker",
            PublisherId = 18,
            Price = 4.99m,
            ReleaseDate = new DateTime(2021, 8, 1),
            Description = "An idle game about making cookies, endlessly.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co8e5m.jpg",
            GameFilePath = "gameFile33.bin"
        };

        var gm35 = new GameEntity
        {
            Name = "20 Minutes Till Dawn",
            PublisherId = 19,
            Price = 4.99m,
            ReleaseDate = new DateTime(2022, 4, 21),
            Description = "A roguelike survival shooter where endless hordes of creatures lurk from the dark.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co4ti3.jpg",
            GameFilePath = "gameFile34.bin"
        };

        var gm36 = new GameEntity
        {
            Name = "VVVVVV",
            PublisherId = 20,
            Price = 4.99m,
            ReleaseDate = new DateTime(2010, 1, 11),
            Description = "A platform game about exploring one simple mechanical idea: reversing gravity instead of jumping.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co4ieg.jpg",
            GameFilePath = "gameFile35.bin"
        };

        var gm37 = new GameEntity
        {
            Name = "One Finger Death Punch",
            PublisherId = 21,
            Price = 4.99m,
            ReleaseDate = new DateTime(2014, 4, 25),
            Description = "A minimalist stick-figure beat-em-up with lightning fast combat using only two buttons.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2h2c.jpg",
            GameFilePath = "gameFile36.bin"
        };

        var gm38 = new GameEntity
        {
            Name = "Downwell",
            PublisherId = 22,
            Price = 2.99m,
            ReleaseDate = new DateTime(2015, 10, 29),
            Description = "A gungoggled, wellbound adventure into the depths of a well, shooting your way down.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co284e.jpg",
            GameFilePath = "gameFile37.bin"
        };

        var gm39 = new GameEntity
        {
            Name = "Hexcells",
            PublisherId = 23,
            Price = 2.99m,
            ReleaseDate = new DateTime(2013, 12, 16),
            Description = "A minimalist logic puzzle game played on a field of hexagonal cells.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co248b.jpg",
            GameFilePath = "gameFile38.bin"
        };

        var gm40 = new GameEntity
        {
            Name = "Warframe",
            PublisherId = 24,
            Price = 0.00m,
            ReleaseDate = new DateTime(2013, 3, 25),
            Description = "A free-to-play cooperative third-person shooter where players control members of the Tenno, a race of ancient warriors, wielding powerful biomechanical Warframes.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/cocase.jpg",
            GameFilePath = "gameFile39.bin"
        };

        var gm41 = new GameEntity
        {
            Name = "Counter-Strike 2",
            PublisherId = 12,
            Price = 0.00m,
            ReleaseDate = new DateTime(2023, 9, 27),
            Description = "A free-to-play multiplayer tactical shooter and the successor to Counter-Strike: Global Offensive, rebuilt on the Source 2 engine.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/coaczd.jpg",
            GameFilePath = "gameFile40.bin"
        };

        var gm42 = new GameEntity
        {
            Name = "Unturned",
            PublisherId = 25,
            Price = 0.00m,
            ReleaseDate = new DateTime(2017, 7, 7),
            Description = "A free-to-play open-world survival game where players scavenge, build, and fight to survive a zombie apocalypse.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/coaary.jpg",
            GameFilePath = "gameFile41.bin"
        };

        var gm43 = new GameEntity
        {
            Name = "Once Human",
            PublisherId = 26,
            Price = 0.00m,
            ReleaseDate = new DateTime(2024, 7, 9),
            Description = "A free-to-play open-world survival game set in a fractured reality overrun by cosmic horrors, blending crafting, base-building, and combat.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/cocleq.jpg",
            GameFilePath = "gameFile42.bin"
        };

        var gm44 = new GameEntity
        {
            Name = "Dota 2",
            PublisherId = 12,
            Price = 0.00m,
            ReleaseDate = new DateTime(2013, 7, 9),
            Description = "A free-to-play multiplayer online battle arena game where two teams of five battle to destroy the opposing team's Ancient.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/cobfk4.jpg",
            GameFilePath = "gameFile43.bin"
        };

        context.Games.AddRange(gm1, gm2, gm3, gm4, gm5, gm6, gm7, gm9, gm10, gm11, gm12, gm13, gm14, gm15, gm16, gm17, gm18, gm19, gm20, gm21, gm22, gm23, gm24, gm25, gm26, gm27, gm28,
            gm29, gm30, gm31, gm32, gm33, gm34, gm35, gm36, gm37, gm38, gm39, gm40, gm41, gm42, gm43, gm44);
        await context.SaveChangesAsync();
        Console.WriteLine("Dynamic seed: Games added.");

    }
    private static async Task SeedGameGenresAsync(DatabaseContext context)
    {
        if (!await context.GameGenres.AnyAsync())
        {
            var gameIdsByName = await context.Games
                .Select(g => new { g.Id, g.Name })
                .ToDictionaryAsync(g => g.Name, g => g.Id);

            var genreIdsByName = await context.Genres
                .Select(g => new { g.Id, g.Name })
                .ToDictionaryAsync(g => g.Name, g => g.Id);

            var mappings = new Dictionary<string, string[]>
            {
                ["Grand Theft Auto 'San Andreas'"] = ["Action", "Open-World"],
                ["FIFA 19"] = ["Sports", "Simulation"],
                ["Red Dead Redemption 2"] = ["Action", "Open-World"],
                ["The Witcher 3: Wild Hunt"] = ["Role-Playing (RPG)", "Adventure"],
                ["Cyberpunk 2077"] = ["Role-Playing (RPG)", "Open-World"],
                ["Elden Ring"] = ["Role-Playing (RPG)", "Action"],
                ["Hades"] = ["Action", "Adventure"],
                ["BestGame"] = ["Adventure", "Action"],
                ["God of War"] = ["Action", "Adventure"],
                ["God of War Ragnarök"] = ["Action", "Adventure"],
                ["Assassin's Creed Valhalla"] = ["Action", "Open-World"],
                ["Assassin's Creed Odyssey"] = ["Role-Playing (RPG)", "Open-World"],
                ["Resident Evil 4 Remake"] = ["Action", "Horror"],
                ["Resident Evil Village"] = ["Horror", "Survival"],
                ["Dark Souls III"] = ["Role-Playing (RPG)", "Action"],
                ["Sekiro: Shadows Die Twice"] = ["Action", "Adventure"],
                ["Starfield"] = ["Role-Playing (RPG)", "Open-World"],
                ["The Elder Scrolls V: Skyrim"] = ["Role-Playing (RPG)", "Open-World"],
                ["Fallout 4"] = ["Role-Playing (RPG)", "Survival"],
                ["DOOM Eternal"] = ["Action", "Horror"],
                ["Half-Life: Alyx"] = ["Action", "Puzzle"],
                ["Horizon Zero Dawn"] = ["Action", "Role-Playing (RPG)"],
                ["Horizon Forbidden West"] = ["Action", "Role-Playing (RPG)"],
                ["Metal Gear Solid V: The Phantom Pain"] = ["Action", "Strategy"],
                ["Battlefield 1"] = ["Action", "Simulation"],
                ["Battlefield V"] = ["Action", "Simulation"],
                ["Death Stranding"] = ["Adventure", "Simulation"],
                ["Team Fortress 2"] = ["Action"],
                ["Vampire Survivors"] = ["Action", "Survival"],
                ["Among Us"] = ["Strategy", "Simulation"],
                ["Plants vs. Zombies: Game of the Year Edition"] = ["Strategy", "Simulation"],
                ["Peggle Deluxe"] = ["Puzzle"],
                ["Cookie Clicker"] = ["Simulation"],
                ["20 Minutes Till Dawn"] = ["Action", "Survival"],
                ["VVVVVV"] = ["Puzzle", "Adventure"],
                ["One Finger Death Punch"] = ["Action"],
                ["Downwell"] = ["Action", "Adventure"],
                ["Hexcells"] = ["Puzzle"],
                ["Warframe"] = ["Action", "Role-Playing (RPG)"],
                ["Counter-Strike 2"] = ["Action", "Strategy"],
                ["Unturned"] = ["Survival", "Open-World"],
                ["Once Human"] = ["Survival", "Open-World"],
                ["Dota 2"] = ["Strategy", "Action"]
            };

            var gameGenres = new List<GameGenreEntity>();

            foreach (var mapping in mappings)
            {
                if (!gameIdsByName.TryGetValue(mapping.Key, out var gameId))
                    continue;

                foreach (var genreName in mapping.Value)
                {
                    if (!genreIdsByName.TryGetValue(genreName, out var genreId))
                        continue;

                    gameGenres.Add(new GameGenreEntity
                    {
                        GameId = gameId,
                        GenreId = genreId
                    });
                }
            }

            context.GameGenres.AddRange(gameGenres);
            await context.SaveChangesAsync();
            Console.WriteLine($"Dynamic seed: {gameGenres.Count} game-genre relations added.");
        }

        if (!await context.Screenshots.AnyAsync())
        {
            var ss1 = new ScreenshotEntity
            {
                GameId = 3,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar3qms.jpg"
            };

            var ss2 = new ScreenshotEntity
            {
                GameId = 9,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar4u6w.jpg"
            };

            var ss3 = new ScreenshotEntity
            {
                GameId = 5,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar3m0i.jpg"
            };

            var ss4 = new ScreenshotEntity
            {
                GameId = 1,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/arqzf.jpg"
            };

            var ss5 = new ScreenshotEntity
            {
                GameId = 6,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/scagdm.jpg"
            };

            var ss6 = new ScreenshotEntity
            {
                GameId = 7,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar10j1.jpg"
            };

            var ss7 = new ScreenshotEntity
            {
                GameId = 4,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar3lzk.jpg"
            };

            var ss8 = new ScreenshotEntity
            {
                GameId = 2,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar27mk.jpg"
            };
           
            var ss9 = new ScreenshotEntity
            {
                GameId = 1,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/scgoqe.jpg"
            }
        ;

            var ss10 = new ScreenshotEntity
            {
                GameId = 18,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar91m.jpg"
            };

            var ss11 = new ScreenshotEntity
            {
                GameId = 11,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar746.jpg"
            };

            var ss12 = new ScreenshotEntity
            {
                GameId = 14,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar814.jpg"
            };

            var ss13 = new ScreenshotEntity
            {
                GameId = 8,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar5jg4.jpg"
            };

            var ss14 = new ScreenshotEntity
            {
                GameId = 10,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar3se9.jpg"
            };

            var ss15 = new ScreenshotEntity
            {
                GameId = 12,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/gaynboyx7mlsgoudkh8a.jpg"
            };

            var ss16 = new ScreenshotEntity
            {
                GameId = 13,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar1xhu.jpg"
            };

            var ss17 = new ScreenshotEntity
            {
                GameId = 15,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/v2pavwp088owka1apo7i.jpg"
            };

            var ss18 = new ScreenshotEntity
            {
                GameId = 16,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ozcogbafdzztevnidcvt.jpg"
            };

            var ss19 = new ScreenshotEntity
            {
                GameId = 17,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ygiajg3ypevvmhoo0cea.jpg"
            };

            var ss20 = new ScreenshotEntity
            {
                GameId = 19,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar103h.jpg"
            };

            var ss21 = new ScreenshotEntity
            {
                GameId = 20,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar6dy.jpg"
            };

            var ss22 = new ScreenshotEntity
            {
                GameId = 21,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar6d8.jpg"
            };

            var ss23 = new ScreenshotEntity
            {
                GameId = 22,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/llwixfa0dy8fpex3tcwr.jpg"
            };

            var ss24 = new ScreenshotEntity
            {
                GameId = 23,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/sc8c96.jpg"
            };

            var ss25 = new ScreenshotEntity
            {
                GameId = 24,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar546.jpg"
            };

            var ss26 = new ScreenshotEntity
            {
                GameId = 25,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar482.jpg"
            };

            var ss27 = new ScreenshotEntity
            {
                GameId = 26,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/meerv4jrmm8orqlmk3dk.jpg"
            };

            var ss28 = new ScreenshotEntity
            {
                GameId = 27,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar3s0u.jpg"
            };

            var ss29 = new ScreenshotEntity
            {
                GameId = 28,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/scnb82.jpg"
            };

            var ss30 = new ScreenshotEntity
            {
                GameId = 29,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar42su.jpg"
            };

            var ss31 = new ScreenshotEntity
            {
                GameId = 30,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar63op.jpg"
            };

            var ss32 = new ScreenshotEntity
            {
                GameId = 31,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar8c0.jpg"
            };

            var ss33 = new ScreenshotEntity
            {
                GameId = 32,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/arnz6.jpg"
            };

            var ss34 = new ScreenshotEntity
            {
                GameId = 33,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar11pk.jpg"
            };

            var ss35 = new ScreenshotEntity
            {
                GameId = 34,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar1mbz.jpg"
            };

            var ss36 = new ScreenshotEntity
            {
                GameId = 35,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/arct2.jpg"
            };

            var ss37 = new ScreenshotEntity
            {
                GameId = 37,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar6vt.jpg"
            };

            var ss38 = new ScreenshotEntity
            {
                GameId = 38,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar5u9i.jpg"
            };

            var ss39 = new ScreenshotEntity
            {
                GameId = 39,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar6595.jpg"
            };

            var ss40 = new ScreenshotEntity
            {
                GameId = 40,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar439t.jpg"
            };

            var ss41 = new ScreenshotEntity
            {
                GameId = 41,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar69q9.jpg"
            };

            var ss42 = new ScreenshotEntity
            {
                GameId = 42,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar6cdn.jpg"
            };

            var ss43 = new ScreenshotEntity
            {
                GameId = 43,
                ImageURL = "https://images.igdb.com/igdb/image/upload/t_1080p/ar31xh.jpg"
            };

            context.Screenshots.AddRange(ss1, ss2, ss3, ss4, ss5, ss6, ss7, ss8, ss9, ss10, ss11, ss12,
                ss13, ss14, ss15, ss16, ss17, ss18, ss19, ss20, ss21, ss22, ss23, ss24, ss25, ss26, ss27, ss28,
                ss29, ss30, ss31, ss32, ss33, ss34, ss35, ss36, ss37, ss38, ss39, ss40, ss41, ss42, ss43);
            await context.SaveChangesAsync();
            Console.WriteLine("Dynamic seed: Screenshots added.");
        }
    }


    private static readonly string[] EditorsPickGameNames =
    [
        "Dark Souls III",
        "Elden Ring",
        "Cyberpunk 2077",
        "DOOM Eternal",
        "Half-Life: Alyx"
    ];

    private static readonly string[] TopRatedThisWeekGameNames =
    [
        "Red Dead Redemption 2",
        "The Witcher 3: Wild Hunt",
        "God of War",
        "Resident Evil Village",
        "Horizon Zero Dawn"
    ];

    private static async Task SeedGameReviewsAsync(DatabaseContext context)
    {
        if (await context.Reviews.AnyAsync())
            return;

        var gameIdsByName = await context.Games
            .Select(g => new { g.Id, g.Name })
            .ToDictionaryAsync(g => g.Name, g => g.Id);

        var reviewerIds = await context.Users
            .Where(u => u.Username.StartsWith("Reviewer"))
            .OrderBy(u => u.Username)
            .Select(u => u.Id)
            .ToListAsync();

        if (reviewerIds.Count == 0)
            return;

        var ratings = new[] { 5f, 4.5f, 5f, 4.5f, 5f };
        var userGames = new List<UserGameEntity>();

        void AddReviews(string gameName, Func<int, DateTime> dateForIndex)
        {
            if (!gameIdsByName.TryGetValue(gameName, out var gameId))
                return;

            for (var i = 0; i < reviewerIds.Count; i++)
            {
                var reviewDate = dateForIndex(i);

                userGames.Add(new UserGameEntity
                {
                    UserId = reviewerIds[i],
                    GameId = gameId,
                    PurchaseDate = reviewDate.AddDays(-1),
                    Review = new ReviewEntity
                    {
                        Rating = ratings[i],
                        Content = "Great game, highly recommended!",
                        Date = reviewDate
                    }
                });
            }
        }

        foreach (var gameName in EditorsPickGameNames)
        {
            AddReviews(gameName, i => DateTime.UtcNow.AddDays(-30 - i));
        }

        foreach (var gameName in TopRatedThisWeekGameNames)
        {
            AddReviews(gameName, i => DateTime.UtcNow.AddDays(-i));
        }

        context.UserGames.AddRange(userGames);
        await context.SaveChangesAsync();

        Console.WriteLine($"Dynamic seed: {userGames.Count} game reviews added.");
    }
}
