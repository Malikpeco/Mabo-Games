using Market.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace Market.Infrastructure.Database.Seeders;

public static class DynamicDataSeeder
{
    public static async Task SeedAsync(DatabaseContext context)
    {
        // Ensure DB is created (no migrations)
        await context.Database.EnsureCreatedAsync();

        await SeedCountriesAsync(context);
        await SeedUsersAsync(context);
        await SeedSecurityQuestionsAsync(context);
        await SeedPublishersAsync(context);
        await SeedGenresAsync(context);
        await SeedGamesAsync(context);
        await SeedGameGenresAsync(context);
        await SeedIGDBToken(context);
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

        context.Publishers.AddRange(pub1, pub2, pub3, pub4, pub5, pub6, pub7, pub8, pub9, pub10, pub11, pub12, pub13, pub14);
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
            Price = 60.00m,
            ReleaseDate = new DateTime(2013, 9, 17),
            Description = "An open-world action-adventure game following three criminals in the fictional state of San Andreas.",
            CoverImageURL = "https://upload.wikimedia.org/wikipedia/en/thumb/c/c4/GTASABOX.jpg/250px-GTASABOX.jpg"
        };

        var gm2 = new GameEntity
        {
            Name = "FIFA 19",
            PublisherId = 2,
            Price = 25.00m,
            ReleaseDate = new DateTime(2018, 9, 28),
            Description = "A football simulation game featuring realistic gameplay, official leagues and teams, and the conclusion of The Journey story mode.",
            CoverImageURL = "https://legacymedia.sportsplatform.io/img/images/photos/003/757/965/75da9a20a992ae7b8b1d18f6ee3fb8a4_crop_north.jpg?w=802"
        };

        var gm3 = new GameEntity
        {
            Name = "Red Dead Redemption 2",
            PublisherId = 1,
            Price = 30.00m,
            ReleaseDate = new DateTime(2018, 10, 26),
            Description = "An epic tale of outlaw Arthur Morgan and the Van der Linde gang in the dying days of the Wild West.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co1q1f.jpg"
        };

        var gm4 = new GameEntity
        {
            Name = "The Witcher 3: Wild Hunt",
            PublisherId = 3,
            Price = 20.00m,
            ReleaseDate = new DateTime(2015, 5, 19),
            Description = "A story-driven open world RPG set in a visually stunning fantasy universe full of meaningful choices.",
            CoverImageURL = "https://cdn.cloudflare.steamstatic.com/steam/apps/292030/header.jpg"
        };

        var gm5 = new GameEntity
        {
            Name = "Cyberpunk 2077",
            PublisherId = 3,
            Price = 27.50m,
            ReleaseDate = new DateTime(2020, 12, 10),
            Description = "An open-world action-adventure story set in Night City, a megalopolis obsessed with power and glamour.",
            CoverImageURL = "https://cdn.cloudflare.steamstatic.com/steam/apps/1091500/header.jpg"
        };

        var gm6 = new GameEntity
        {
            Name = "Elden Ring",
            PublisherId = 4,
            Price = 40.00m,
            ReleaseDate = new DateTime(2022, 2, 25),
            Description = "A vast action RPG world filled with mystery and danger, created by Hidetaka Miyazaki and George R. R. Martin.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co4jni.jpg"
        };

        var gm7 = new GameEntity
        {
            Name = "Hades",
            PublisherId = 6,
            Price = 12.50m,
            ReleaseDate = new DateTime(2020, 9, 17),
            Description = "A rogue-like dungeon crawler where you defy the god of the dead while wielding mythic weapons.",
            CoverImageURL = "https://upload.wikimedia.org/wikipedia/en/c/cc/Hades_cover_art.jpg"
        };



        var gm9 = new GameEntity
        {
            Name = "Tomb Raider",
            PublisherId = 1,
            Price = 2.50m,
            ReleaseDate = new DateTime(2026, 1, 2),
            Description = "this is a test game boi",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co1rbu.jpg"
        };

        var gm10 = new GameEntity
        {
            Name = "God of War",
            PublisherId = 5,
            Price = 30.00m,
            ReleaseDate = new DateTime(2018, 4, 20),
            Description = "A mythological action-adventure following Kratos in Norse lands.",
            CoverImageURL = "https://upload.wikimedia.org/wikipedia/en/a/a7/God_of_War_4_cover.jpg"
        };

        var gm11 = new GameEntity
        {
            Name = "God of War Ragnarök",
            PublisherId = 5,
            Price = 45.00m,
            ReleaseDate = new DateTime(2022, 11, 9),
            Description = "The epic continuation of Kratos and Atreus’ Norse saga.",
            CoverImageURL = "https://upload.wikimedia.org/wikipedia/en/e/ee/God_of_War_Ragnar%C3%B6k_cover.jpg"
        };

        var gm12 = new GameEntity
        {
            Name = "Assassin's Creed Valhalla",
            PublisherId = 8,
            Price = 35.00m,
            ReleaseDate = new DateTime(2020, 11, 10),
            Description = "An open-world Viking adventure set in Dark Ages England.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2ed3.jpg"
        };

        var gm13 = new GameEntity
        {
            Name = "Assassin's Creed Odyssey",
            PublisherId = 8,
            Price = 0.00m,
            ReleaseDate = new DateTime(2018, 10, 5),
            Description = "Explore ancient Greece in this vast open-world RPG.",
            CoverImageURL = "https://upload.wikimedia.org/wikipedia/en/9/99/ACOdysseyCoverArt.png"
        };

        var gm14 = new GameEntity
        {
            Name = "Resident Evil 4 Remake",
            PublisherId = 9,
            Price = 42.50m,
            ReleaseDate = new DateTime(2023, 3, 24),
            Description = "A modern reimagining of the legendary survival horror game.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co6bo0.jpg"
        };

        var gm15 = new GameEntity
        {
            Name = "Resident Evil Village",
            PublisherId = 9,
            Price = 22.50m,
            ReleaseDate = new DateTime(2021, 5, 7),
            Description = "Survival horror set in a mysterious European village.",
            CoverImageURL = "https://upload.wikimedia.org/wikipedia/en/2/2c/Resident_Evil_Village.png"
        };

        var gm16 = new GameEntity
        {
            Name = "Dark Souls III",
            PublisherId = 4,
            Price = 0.00m,
            ReleaseDate = new DateTime(2016, 4, 12),
            Description = "A challenging action RPG set in a dark fantasy world.",
            CoverImageURL = "https://upload.wikimedia.org/wikipedia/en/b/bb/Dark_souls_3_cover_art.jpg"
        };

        var gm17 = new GameEntity
        {
            Name = "Sekiro: Shadows Die Twice",
            PublisherId = 4,
            Price = 32.50m,
            ReleaseDate = new DateTime(2019, 3, 22),
            Description = "A precision-based action game set in feudal Japan.",
            CoverImageURL = "https://upload.wikimedia.org/wikipedia/en/6/6e/Sekiro_art.jpg"
        };

        var gm18 = new GameEntity
        {
            Name = "Starfield",
            PublisherId = 10,
            Price = 50.00m,
            ReleaseDate = new DateTime(2023, 9, 6),
            Description = "A massive space RPG exploring the vastness of the universe.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co39vv.jpg"
        };

        var gm19 = new GameEntity
        {
            Name = "The Elder Scrolls V: Skyrim",
            PublisherId = 10,
            Price = 15.00m,
            ReleaseDate = new DateTime(2011, 11, 11),
            Description = "An open-world fantasy RPG set in the land of Skyrim.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co1tnw.jpg"
        };

        var gm20 = new GameEntity
        {
            Name = "Fallout 4",
            PublisherId = 10,
            Price = 12.50m,
            ReleaseDate = new DateTime(2015, 11, 10),
            Description = "A post-apocalyptic RPG set in the ruins of Boston.",
            CoverImageURL = "https://image.api.playstation.com/vulcan/ap/rnd/202009/2502/rB3GRFvdPmaALiGt89ysflQ4.jpg"
        };

        var gm21 = new GameEntity
        {
            Name = "DOOM Eternal",
            PublisherId = 10,
            Price = 22.50m,
            ReleaseDate = new DateTime(2020, 3, 20),
            Description = "Fast-paced demon-slaying FPS action.",
            CoverImageURL = "https://www.theouterhaven.net/wp-content/uploads/2020/02/doom-eternal-2020-top-625x352-1.jpg"
        };

        var gm22 = new GameEntity
        {
            Name = "Half-Life: Alyx",
            PublisherId = 11,
            Price = 0.00m,
            ReleaseDate = new DateTime(2020, 3, 23),
            Description = "A VR return to the Half-Life universe.",
            CoverImageURL = "https://upload.wikimedia.org/wikipedia/en/4/49/Half-Life_Alyx_Cover_Art.jpg"
        };

        var gm23 = new GameEntity
        {
            Name = "Horizon Zero Dawn",
            PublisherId = 5,
            Price = 20.00m,
            ReleaseDate = new DateTime(2017, 2, 28),
            Description = "An open-world action RPG in a post-apocalyptic world.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2una.jpg"
        };

        var gm24 = new GameEntity
        {
            Name = "Horizon Forbidden West",
            PublisherId = 5,
            Price = 40.00m,
            ReleaseDate = new DateTime(2022, 2, 18),
            Description = "The continuation of Aloy’s journey in a dangerous frontier.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2gvu.jpg"
        };

        var gm25 = new GameEntity
        {
            Name = "Metal Gear Solid V: The Phantom Pain",
            PublisherId = 13,
            Price = 10.00m,
            ReleaseDate = new DateTime(2015, 9, 1),
            Description = "A tactical stealth game with an open-world design.",
            CoverImageURL = "https://upload.wikimedia.org/wikipedia/en/8/8f/Metal_Gear_Solid_V_The_Phantom_Pain_cover.png"
        };

        var gm26 = new GameEntity
        {
            Name = "Battlefield 1",
            PublisherId = 2,
            Price = 17.50m,
            ReleaseDate = new DateTime(2016, 10, 21),
            Description = "A World War I themed first-person shooter.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2n9d.jpg"
        };

        var gm27 = new GameEntity
        {
            Name = "Battlefield V",
            PublisherId = 2,
            Price = 0.00m,
            ReleaseDate = new DateTime(2018, 11, 20),
            Description = "A WWII shooter focused on large-scale battles.",
            CoverImageURL = "https://images.igdb.com/igdb/image/upload/t_cover_big/co1xbv.jpg"
        };

        var gm28 = new GameEntity
        {
            Name = "Death Stranding",
            PublisherId = 5,
            Price = 22.50m,
            ReleaseDate = new DateTime(2019, 11, 8),
            Description = "A unique narrative-driven experience in a fractured world.",
            CoverImageURL = "https://upload.wikimedia.org/wikipedia/en/2/22/Death_Stranding.jpg"
        };

        context.Games.AddRange(gm1, gm2, gm3, gm4, gm5, gm6, gm7, gm9, gm10, gm11, gm12, gm13, gm14, gm15, gm16, gm17, gm18, gm19, gm20, gm21, gm22, gm23, gm24, gm25, gm26, gm27, gm28);
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
                ["Death Stranding"] = ["Adventure", "Simulation"]
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
                ImageURL = "https://cdn1.epicgames.com/b30b6d1b4dfd4dcc93b5490be5e094e5/offer/RDR2476298253_Epic_Games_Wishlist_RDR2_2560x1440_V01-2560x1440-2a9ebe1f7ee202102555be202d5632ec.jpg"
            };

            var ss2 = new ScreenshotEntity
            {
                GameId = 9,
                ImageURL = "https://static0.howtogeekimages.com/wordpress/wp-content/uploads/2018/01/img_5a6791a231406.jpg"
            };

            var ss3 = new ScreenshotEntity
            {
                GameId = 5,
                ImageURL = "https://www.escapistmagazine.com/wp-content/uploads/2019/07/GOW-Feature-Image.jpg"
            };

            var ss4 = new ScreenshotEntity
            {
                GameId = 1,
                ImageURL = "https://www.igta5.com/images/official-screenshot-cant-touch-this.jpg"
            };

            var ss5 = new ScreenshotEntity
            {
                GameId = 6,
                ImageURL = "https://cdn.mos.cms.futurecdn.net/8gWTFzyHLQXnTGiVhRLeea.jpg"
            };

            var ss6 = new ScreenshotEntity
            {
                GameId = 7,
                ImageURL = "https://assets.nintendo.com/image/upload/q_auto/f_auto/store/software/switch/70010000033131/dbc8c55a21688b446a5c57711b726956483a14ef8c5ddb861f897c0595ccb6b5"
            };

            var ss7 = new ScreenshotEntity
            {
                GameId = 4,
                ImageURL = "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/292030/ad9240e088f953a84aee814034c50a6a92bf4516/header.jpg?t=1765462356"
            };

            var ss8 = new ScreenshotEntity
            {
                GameId = 2,
                ImageURL = "https://static.standard.co.uk/s3fs-public/thumbnails/image/2018/09/06/16/easportsfifa19.jpg?width=1200"
            };

            var ss9 = new ScreenshotEntity
            {
                GameId = 1,
                ImageURL = "https://upload.wikimedia.org/wikipedia/en/thumb/c/c4/GTASABOX.jpg/250px-GTASABOX.jpg"
            };

            var ss10 = new ScreenshotEntity
            {
                GameId = 18,
                ImageURL = "https://www.newgamenetwork.com/app/uploads/2025/10/starfield_07_3.jpg"
            };

            var ss11 = new ScreenshotEntity
            {
                GameId = 11,
                ImageURL = "https://gamingbolt.com/wp-content/uploads/2022/10/god-of-war-ragnarok-image.jpg"
            };

            var ss12 = new ScreenshotEntity
            {
                GameId = 14,
                ImageURL = "https://img.playstationtrophies.org/images/monthly_2024_02/screenshots/16476/re4_ge_01_51b1e450-b26b-4e8a-891c-cb68a8138fa6.jpg"
            };

            context.Screenshots.AddRange(ss1, ss2, ss3, ss4, ss5, ss6, ss7, ss8, ss9, ss10, ss11, ss12);
            await context.SaveChangesAsync();
            Console.WriteLine("Dynamic seed: Screenshots added.");
        }
    }


}
