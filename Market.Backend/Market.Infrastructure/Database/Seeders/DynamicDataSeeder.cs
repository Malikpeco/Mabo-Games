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
        await SeedSecurityQuestionsAsync(context);
    }

    private static async Task SeedCountriesAsync(DatabaseContext context)
    {
        if (await context.Countries.AnyAsync())
            return;

        var bih = new CountryEntity
        {
            Name = "Bosna i Hercegovina"
        };

        var cro = new CountryEntity
        {
            Name = "Hrvatska"
        };

        context.Countries.AddRange(bih, cro);
        await context.SaveChangesAsync();

        Console.WriteLine("Dynamic seed: Countries added.");
    }

    private static async Task SeedSecurityQuestionsAsync(DatabaseContext context)
    {
        if (await context.SecurityQuestions.AnyAsync())
            return;

        var questionOne = new SecurityQuestionEntity
        {
            Question = "What was the name of your firs pet?"
        };

        var questionTwo = new SecurityQuestionEntity
        {
            Question = "What was your childhood nickname?"
        };

        var questionThree = new SecurityQuestionEntity
        {
            Question = "In what city were you born?"
            
        };


        context.SecurityQuestions.AddRange(questionOne,questionTwo,questionThree );
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
}
