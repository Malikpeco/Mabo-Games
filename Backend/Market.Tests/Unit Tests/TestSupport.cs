using Market.Application.Abstractions;
using Market.Domain.Entities;
using Market.Domain.Entities.Identity;
using Microsoft.Extensions.Time.Testing;

namespace Market.Tests.UnitTests;

internal static class TestSupport
{
    public static DatabaseContext CreateContext() => new(
        new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options,
        new FakeTimeProvider(new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero)));

    public static PublisherEntity Publisher(string name = "Test Publisher") => new()
    {
        Name = name, Country = new CountryEntity { Name = "Test Country" }
    };

    public static GameEntity Game(string name = "Test Game") => new()
    {
        Name = name, Price = 20, Publisher = Publisher(), GameFilePath = "games/test.zip",
        ReleaseDate = new DateTime(2025, 1, 1)
    };

    public static UserEntity User(string username = "testuser") => new()
    {
        Username = username, Email = username + "@example.com", PasswordHash = "test-hash",
        FirstName = "Test", LastName = "User", IsEnabled = true, ProfileBio = "Old bio"
    };
}

internal sealed class TestCurrentUser : IAppCurrentUser
{
    public int? UserId { get; init; } = 1;
    public string? Email { get; init; } = "admin@example.com";
    public bool IsAuthenticated { get; init; } = true;
    public bool IsAdmin { get; init; } = true;
}

internal sealed class TestStorage : ISupaBaseService
{
    public List<string> UploadedFiles { get; } = new();
    public List<string> DeletedFiles { get; } = new();
    public bool FailUpload { get; init; }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, CancellationToken ct = default)
    {
        if (FailUpload) throw new IOException("Upload failed");
        await fileStream.CopyToAsync(Stream.Null, ct);
        UploadedFiles.Add(fileName);
        return "games/" + fileName;
    }

    public Task DeleteFileAsync(string filePath, CancellationToken ct = default)
    {
        DeletedFiles.Add(filePath);
        return Task.CompletedTask;
    }

    public Task<string> GetSignedUrlAsync(string filePath, int expiresInSeconds = 3600, CancellationToken ct = default)
        => throw new NotSupportedException();
}

internal sealed class TestEmailSender : IEmailSender
{
    public List<(string Recipient, string Subject, string Body)> Messages { get; } = new();

    public Task SendEmail(string recieverEmail, string subject, string emailText, CancellationToken cancellationTokens)
    {
        Messages.Add((recieverEmail, subject, emailText));
        return Task.CompletedTask;
    }

    public Task SendPasswordRecoveryCode(string recieverEmail, string recoverCode, CancellationToken cancellationTokens)
        => throw new NotSupportedException();
}
