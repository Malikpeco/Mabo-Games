using Market.Application.Common.Exceptions;
using Market.Application.Modules.Users.Commands.ChangeBio;
using Market.Application.Modules.Users.Commands.ChangeUsername;
using Market.Application.Modules.Users.Commands.DeleteUser;
using Market.Application.Modules.Users.Queries.GetUserProfileQuery;
using Market.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Market.Tests.UnitTests;

public class UserUnitTests
{
    [Theory]
    [InlineData("New bio")]
    [InlineData(null)]
    [InlineData("Old bio")]
    public async Task ChangeBio_ShouldPersistRequestedBioOnlyForCurrentUser(string? bio)
    {
        // Arrange
        using var context = TestSupport.CreateContext();
        var user = TestSupport.User();
        var other = TestSupport.User("otheruser");
        context.Users.AddRange(user, other);
        await context.SaveChangesAsync();
        var handler = new ChangeProfileBioCommandHandler(context, new TestCurrentUser { UserId = user.Id });

        // Act
        await handler.Handle(new ChangeProfileBioCommand { NewBio = bio }, CancellationToken.None);

        // Assert
        context.ChangeTracker.Clear();
        Assert.Equal(bio, (await context.Users.SingleAsync(u => u.Id == user.Id)).ProfileBio);
        Assert.Equal("Old bio", (await context.Users.SingleAsync(u => u.Id == other.Id)).ProfileBio);
    }

    [Fact]
    public async Task ChangeBio_WhenUnauthenticated_ShouldReject()
    {
        using var context = TestSupport.CreateContext();
        var handler = new ChangeProfileBioCommandHandler(context, new TestCurrentUser { IsAuthenticated = false, UserId = null });
        await Assert.ThrowsAsync<MarketForbiddenException>(() => handler.Handle(
            new ChangeProfileBioCommand { NewBio = "New bio" }, CancellationToken.None));
    }

    [Fact]
    public async Task ChangeUsername_ShouldPersistAndSendNotification()
    {
        using var context = TestSupport.CreateContext();
        var user = TestSupport.User();
        var hasher = new PasswordHasher<UserEntity>();
        user.PasswordHash = hasher.HashPassword(user, "CorrectPassword123!");
        context.Users.Add(user);
        await context.SaveChangesAsync();
        var email = new TestEmailSender();
        var handler = new ChangeUsernameCommandHandler(context, new TestCurrentUser { UserId = user.Id }, email, hasher);

        await handler.Handle(new ChangeUsernameCommand { Password = "CorrectPassword123!", NewUsername = "newusername" }, CancellationToken.None);

        context.ChangeTracker.Clear();
        Assert.Equal("newusername", (await context.Users.SingleAsync()).Username);
        var message = Assert.Single(email.Messages);
        Assert.Equal(user.Email, message.Recipient);
        Assert.Equal("Username Change", message.Subject);
        Assert.Contains("testuser", message.Body);
        Assert.Contains("newusername", message.Body);
    }

    [Theory]
    [InlineData("WrongPassword", "newusername", false)]
    [InlineData("CorrectPassword123!", "testuser", true)]
    [InlineData("CorrectPassword123!", "otheruser", true)]
    public async Task ChangeUsername_WhenInvalid_ShouldKeepUsernameAndNotEmail(string password, string username, bool conflict)
    {
        using var context = TestSupport.CreateContext();
        var user = TestSupport.User();
        var hasher = new PasswordHasher<UserEntity>();
        user.PasswordHash = hasher.HashPassword(user, "CorrectPassword123!");
        context.Users.AddRange(user, TestSupport.User("otheruser"));
        await context.SaveChangesAsync();
        var email = new TestEmailSender();
        var handler = new ChangeUsernameCommandHandler(context, new TestCurrentUser { UserId = user.Id }, email, hasher);
        var command = new ChangeUsernameCommand { Password = password, NewUsername = username };

        if (conflict)
            await Assert.ThrowsAsync<MarketConflictException>(() => handler.Handle(command, CancellationToken.None));
        else
            await Assert.ThrowsAsync<MarketForbiddenException>(() => handler.Handle(command, CancellationToken.None));

        context.ChangeTracker.Clear();
        Assert.Equal("testuser", (await context.Users.SingleAsync(u => u.Id == user.Id)).Username);
        Assert.Empty(email.Messages);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GetProfile_ShouldIdentifyOwnProfile(bool ownProfile)
    {
        using var context = TestSupport.CreateContext();
        var user = TestSupport.User();
        context.Users.Add(user);
        await context.SaveChangesAsync();
        var handler = new GetUserProfileQueryHandler(new TestCurrentUser { UserId = ownProfile ? user.Id : null }, context);

        var result = await handler.Handle(new GetUserProfileQuery(user.Username), CancellationToken.None);

        Assert.Equal(user.Username, result.Username);
        Assert.Equal(user.ProfileBio, result.Bio);
        Assert.Equal(ownProfile, result.IsOwnProfile);
        Assert.Equal(0, result.OwnedGamesCount);
        Assert.Empty(result.RecentlyBoughtGames);
    }

    [Fact]
    public async Task GetProfile_WhenMissing_ShouldThrowNotFound()
    {
        using var context = TestSupport.CreateContext();
        var handler = new GetUserProfileQueryHandler(new TestCurrentUser(), context);
        await Assert.ThrowsAsync<MarketNotFoundException>(() => handler.Handle(new GetUserProfileQuery("missing"), CancellationToken.None));
    }

    [Fact]
    public async Task Delete_ShouldDisableSoftDeleteAndNotifyUser()
    {
        using var context = TestSupport.CreateContext();
        var user = TestSupport.User();
        user.TokenVersion = 4;
        context.Users.Add(user);
        await context.SaveChangesAsync();
        var email = new TestEmailSender();
        var handler = new DeleteUserCommandHandler(context, new TestCurrentUser { UserId = user.Id }, email);

        await handler.Handle(new DeleteUserCommand { ConfirmationText = "DELETE MY ACCOUNT" }, CancellationToken.None);

        context.ChangeTracker.Clear();
        Assert.Empty(await context.Users.ToListAsync());
        var saved = await context.Users.IgnoreQueryFilters().SingleAsync();
        Assert.True(saved.IsDeleted);
        Assert.False(saved.IsEnabled);
        Assert.Equal(5, saved.TokenVersion);
        Assert.Equal(user.Email, Assert.Single(email.Messages).Recipient);
    }

    [Fact]
    public async Task Delete_WhenMissing_ShouldNotEmail()
    {
        using var context = TestSupport.CreateContext();
        var email = new TestEmailSender();
        var handler = new DeleteUserCommandHandler(context, new TestCurrentUser { UserId = 999 }, email);
        await Assert.ThrowsAsync<MarketNotFoundException>(() => handler.Handle(
            new DeleteUserCommand { ConfirmationText = "DELETE MY ACCOUNT" }, CancellationToken.None));
        Assert.Empty(email.Messages);
    }

    [Theory]
    [InlineData(false, 1)]
    [InlineData(true, null)]
    public async Task Delete_WhenNotIdentified_ShouldReject(bool authenticated, int? userId)
    {
        using var context = TestSupport.CreateContext();
        var email = new TestEmailSender();
        var handler = new DeleteUserCommandHandler(context,
            new TestCurrentUser { IsAuthenticated = authenticated, UserId = userId }, email);
        await Assert.ThrowsAsync<MarketForbiddenException>(() => handler.Handle(
            new DeleteUserCommand { ConfirmationText = "DELETE MY ACCOUNT" }, CancellationToken.None));
        Assert.Empty(email.Messages);
    }
}
