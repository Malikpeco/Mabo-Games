using Market.Application.Common.Exceptions;
using Market.Application.Modules.Games.Commands.Create;
using Market.Application.Modules.Games.Commands.Delete;
using Market.Application.Modules.Games.Commands.Update;
using Market.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Market.Tests.UnitTests;

public class GameUnitTests
{
    private static CreateGameCommand CreateCommand(int publisherId) => new()
    {
        Name = "  New Game  ", Price = 29.99m, Description = "Game description",
        ReleaseDate = new DateTime(2025, 1, 1), PublisherId = publisherId,
        CoverImageURL = "https://example.com/cover.png",
        File = new FormFile(new MemoryStream(new byte[] { 1, 2, 3 }), 0, 3, "File", "new.zip")
    };

    [Fact]
    public async Task Create_ShouldPersistGameGenresScreenshotsAndUploadedPath()
    {
        // Arrange
        using var context = TestSupport.CreateContext();
        var publisher = TestSupport.Publisher();
        var genre = new GenreEntity { Name = "Adventure" };
        context.AddRange(publisher, genre);
        await context.SaveChangesAsync();
        var storage = new TestStorage();
        var handler = new CreateGameCommandHandler(context, new TestCurrentUser(), storage);
        var command = CreateCommand(publisher.Id);
        command.GenreIds.Add(genre.Id);
        command.ScreenshotUrls.Add("https://example.com/screenshot.png");

        // Act
        var id = await handler.Handle(command, CancellationToken.None);

        // Assert
        context.ChangeTracker.Clear();
        var game = await context.Games.Include(g => g.GameGenres).Include(g => g.Screenshots).SingleAsync(g => g.Id == id);
        Assert.Equal("New Game", game.Name);
        Assert.Equal(command.Price, game.Price);
        Assert.Equal(command.Description, game.Description);
        Assert.Equal(command.ReleaseDate, game.ReleaseDate);
        Assert.Equal(command.CoverImageURL, game.CoverImageURL);
        Assert.Equal(publisher.Id, game.PublisherId);
        Assert.Equal("games/new.zip", game.GameFilePath);
        Assert.Equal(genre.Id, Assert.Single(game.GameGenres).GenreId);
        Assert.Equal(command.ScreenshotUrls[0], Assert.Single(game.Screenshots).ImageURL);
        Assert.Equal("new.zip", Assert.Single(storage.UploadedFiles));
    }

    [Fact]
    public async Task Create_WhenNameExistsIgnoringCaseAndWhitespace_ShouldNotUpload()
    {
        using var context = TestSupport.CreateContext();
        var game = TestSupport.Game("New Game");
        context.Games.Add(game);
        await context.SaveChangesAsync();
        var storage = new TestStorage();
        var handler = new CreateGameCommandHandler(context, new TestCurrentUser(), storage);
        var command = CreateCommand(game.PublisherId);
        command.Name = "  NEW GAME  ";

        await Assert.ThrowsAsync<MarketConflictException>(() => handler.Handle(command, CancellationToken.None));
        Assert.Empty(storage.UploadedFiles);
        Assert.Equal(1, await context.Games.CountAsync());
    }

    [Fact]
    public async Task Create_WhenPublisherMissing_ShouldNotUploadOrSave()
    {
        using var context = TestSupport.CreateContext();
        var storage = new TestStorage();
        var handler = new CreateGameCommandHandler(context, new TestCurrentUser(), storage);

        await Assert.ThrowsAsync<MarketNotFoundException>(() => handler.Handle(CreateCommand(999), CancellationToken.None));
        Assert.Empty(storage.UploadedFiles);
        Assert.Empty(await context.Games.ToListAsync());
    }

    [Fact]
    public async Task Create_WhenNotAdmin_ShouldNotUploadOrSave()
    {
        using var context = TestSupport.CreateContext();
        var storage = new TestStorage();
        var handler = new CreateGameCommandHandler(context, new TestCurrentUser { IsAdmin = false }, storage);

        await Assert.ThrowsAsync<MarketForbiddenException>(() => handler.Handle(CreateCommand(1), CancellationToken.None));
        Assert.Empty(storage.UploadedFiles);
        Assert.Empty(await context.Games.ToListAsync());
    }

    [Fact]
    public async Task Create_WhenUploadFails_ShouldNotSaveGame()
    {
        using var context = TestSupport.CreateContext();
        var publisher = TestSupport.Publisher();
        context.Publishers.Add(publisher);
        await context.SaveChangesAsync();
        var handler = new CreateGameCommandHandler(context, new TestCurrentUser(), new TestStorage { FailUpload = true });

        await Assert.ThrowsAsync<IOException>(() => handler.Handle(CreateCommand(publisher.Id), CancellationToken.None));
        Assert.Empty(await context.Games.ToListAsync());
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Update_ShouldReplaceRelationsAndOnlyReplaceFileWhenProvided(bool replaceFile)
    {
        using var context = TestSupport.CreateContext();
        var game = TestSupport.Game();
        var oldGenre = new GenreEntity { Name = "Action" };
        var newGenre = new GenreEntity { Name = "Adventure" };
        context.AddRange(game, oldGenre, newGenre);
        await context.SaveChangesAsync();
        game.AddGenre(oldGenre.Id);
        game.AddScreenshot("https://example.com/old.png");
        await context.SaveChangesAsync();
        var storage = new TestStorage();
        var handler = new UpdateGameCommandHandler(context, new TestCurrentUser(), storage);
        var command = new UpdateGameCommand
        {
            Id = game.Id, Name = "Updated Game", Price = 15, PublisherId = game.PublisherId,
            ReleaseDate = new DateTime(2025, 2, 1),
            GenreIds = new() { newGenre.Id, newGenre.Id, 0, -1 },
            ScreenshotUrls = new() { " https://example.com/new.png ", "https://example.com/NEW.png", "" },
            File = replaceFile ? CreateCommand(game.PublisherId).File : null
        };

        await handler.Handle(command, CancellationToken.None);

        context.ChangeTracker.Clear();
        var saved = await context.Games.Include(g => g.GameGenres).Include(g => g.Screenshots).SingleAsync();
        Assert.Equal("Updated Game", saved.Name);
        Assert.Equal(15m, saved.Price);
        Assert.Equal(newGenre.Id, Assert.Single(saved.GameGenres).GenreId);
        Assert.Equal("https://example.com/new.png", Assert.Single(saved.Screenshots).ImageURL);
        Assert.Equal(1, await context.GameGenres.CountAsync());
        Assert.Equal(1, await context.Screenshots.CountAsync());
        Assert.Equal(replaceFile ? "games/new.zip" : "games/test.zip", saved.GameFilePath);
        if (replaceFile)
        {
            Assert.Equal("games/test.zip", Assert.Single(storage.DeletedFiles));
            Assert.Equal("new.zip", Assert.Single(storage.UploadedFiles));
        }
        else
        {
            Assert.Empty(storage.DeletedFiles);
            Assert.Empty(storage.UploadedFiles);
        }
    }

    [Fact]
    public async Task Delete_ShouldRemoveFileAndSoftDeleteGame()
    {
        using var context = TestSupport.CreateContext();
        var game = TestSupport.Game();
        context.Games.Add(game);
        await context.SaveChangesAsync();
        var storage = new TestStorage();
        var handler = new DeleteGameCommandHandler(context, new TestCurrentUser(), storage);

        await handler.Handle(new DeleteGameCommand { Id = game.Id }, CancellationToken.None);

        context.ChangeTracker.Clear();
        Assert.Empty(await context.Games.ToListAsync());
        Assert.True((await context.Games.IgnoreQueryFilters().SingleAsync()).IsDeleted);
        Assert.Equal("games/test.zip", Assert.Single(storage.DeletedFiles));
    }

    [Fact]
    public async Task Delete_WhenOwned_ShouldKeepGameAndFile()
    {
        using var context = TestSupport.CreateContext();
        var game = TestSupport.Game();
        context.UserGames.Add(new UserGameEntity { User = TestSupport.User(), Game = game });
        await context.SaveChangesAsync();
        var storage = new TestStorage();
        var handler = new DeleteGameCommandHandler(context, new TestCurrentUser(), storage);

        await Assert.ThrowsAsync<MarketBusinessRuleException>(() => handler.Handle(
            new DeleteGameCommand { Id = game.Id }, CancellationToken.None));
        Assert.Empty(storage.DeletedFiles);
        Assert.True(await context.Games.AnyAsync(g => g.Id == game.Id));
    }

    [Fact]
    public async Task Delete_WhenMissing_ShouldNotDeleteFile()
    {
        using var context = TestSupport.CreateContext();
        var storage = new TestStorage();
        var handler = new DeleteGameCommandHandler(context, new TestCurrentUser(), storage);

        await Assert.ThrowsAsync<MarketNotFoundException>(() => handler.Handle(
            new DeleteGameCommand { Id = 999 }, CancellationToken.None));
        Assert.Empty(storage.DeletedFiles);
    }
}
