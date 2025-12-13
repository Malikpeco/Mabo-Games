using Market.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Market.Infrastructure.Database.Seeders;

public static class DynamicDataSeeder
{
    public static async Task SeedAsync(DatabaseContext context)
    {
        // Ensure DB is created (no migrations)
        await context.Database.EnsureCreatedAsync();

        await SeedCountriesAsync(context);
        await SeedUsersAsync(context);
        await SeedPublishersAsync(context);
        await SeedGamesAsync(context);
    }

    private static async Task SeedCountriesAsync(DatabaseContext context)
    {
        if (await context.Countries.AnyAsync())
            return;

        var usa = new CountryEntity
        {
            Name = "USA"
        };

        var bih = new CountryEntity
        {
            Name = "Bosna i Hercegovina"
        };

        var cro = new CountryEntity
        {
            Name = "Hrvatska"
        };

        context.Countries.AddRange(bih, cro, usa);
        await context.SaveChangesAsync();

        Console.WriteLine("Dynamic seed: Countries added.");
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
            CountryId = defaultCountry.Id
        };

        admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");


        var user = new UserEntity
        {
            Username = "User",
            Email = "User@market.com",
            FirstName = "User",
            LastName = "User",
            IsAdmin = false,
            IsEnabled = true,
            CreationDate = DateTime.UtcNow,
            CountryId = defaultCountry.Id
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
            CountryId=1
        };

        var pub2 = new PublisherEntity
        {
            Name = "EA",
            CountryId = 1
        };

        context.Publishers.AddRange(pub1, pub2);
        await context.SaveChangesAsync();

        Console.WriteLine("Dynamic seed: Publishers added.");
    }

    private static async Task SeedGamesAsync(DatabaseContext context)
    {
        if (await context.Games.AnyAsync())
            return;

        var gtaSanAndreas = new GameEntity
        {
            Name = "Grand Theft Auto 'San Andreas'",
            PublisherId = 1,
            Price = 59.99m,
            ReleaseDate = new DateTime(2013, 9, 17),
            Description = "An open-world action-adventure game following three criminals in the fictional state of San Andreas."
        };

        var fifa19 = new GameEntity
        {
            Name = "FIFA 19",
            PublisherId = 2,
            Price = 59.99m,
            ReleaseDate = new DateTime(2018, 9, 28),
            Description = "A football simulation game featuring realistic gameplay, official leagues and teams, and the conclusion of The Journey story mode."
        };

        context.Games.AddRange(gtaSanAndreas, fifa19);
        await context.SaveChangesAsync();
        Console.WriteLine("Dynamic seed: Games added.");

    }
}
