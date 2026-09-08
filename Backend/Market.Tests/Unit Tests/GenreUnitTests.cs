using Market.Application.Common.Exceptions;
using Market.Application.Modules.Genres.Commands.Create;
using Market.Application.Modules.Genres.Commands.Delete;
using Market.Application.Modules.Genres.Commands.Update;
using Market.Application.Modules.Genres.Queries.List;
using Market.Domain.Entities;

namespace Market.Tests.UnitTests;

public class GenreUnitTests
{
    [Fact]
    public async Task Create_ShouldPersistGenre()
    {
        // Arrange
        using var context = TestSupport.CreateContext();
        var handler = new CreateGenreCommandHanlder(new TestCurrentUser(), context);

        // Act
        var id = await handler.Handle(new CreateGenreCommand { Name = "Adventure" }, CancellationToken.None);

        // Assert
        context.ChangeTracker.Clear();
        var genre = await context.Genres.SingleAsync(g => g.Id == id);
        Assert.Equal("Adventure", genre.Name);
        Assert.Equal(new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), genre.CreatedAtUtc);
    }

    [Fact]
    public async Task Create_WhenNameExists_ShouldRejectDuplicate()
    {
        // Arrange
        using var context = TestSupport.CreateContext();
        context.Genres.Add(new GenreEntity { Name = "Adventure" });
        await context.SaveChangesAsync();
        var handler = new CreateGenreCommandHanlder(new TestCurrentUser(), context);

        // Act / Assert
        await Assert.ThrowsAsync<MarketConflictException>(() => handler.Handle(
            new CreateGenreCommand { Name = "Adventure" }, CancellationToken.None));
        Assert.Equal(1, await context.Genres.CountAsync());
    }

    [Fact]
    public async Task Create_WhenNotAdmin_ShouldRejectWithoutSaving()
    {
        using var context = TestSupport.CreateContext();
        var handler = new CreateGenreCommandHanlder(new TestCurrentUser { IsAdmin = false }, context);

        await Assert.ThrowsAsync<MarketForbiddenException>(() => handler.Handle(
            new CreateGenreCommand { Name = "Adventure" }, CancellationToken.None));
        Assert.Empty(await context.Genres.ToListAsync());
    }

    [Fact]
    public async Task Update_ShouldPersistNewName()
    {
        using var context = TestSupport.CreateContext();
        var genre = new GenreEntity { Name = "Adventure" };
        context.Genres.Add(genre);
        await context.SaveChangesAsync();
        var handler = new UpdateGenreCommandHandler(context, new TestCurrentUser());

        await handler.Handle(new UpdateGenreCommand { Id = genre.Id, Name = "Action" }, CancellationToken.None);

        context.ChangeTracker.Clear();
        Assert.Equal("Action", (await context.Genres.SingleAsync()).Name);
    }

    [Fact]
    public async Task Update_WhenOtherNameExistsIgnoringCase_ShouldReject()
    {
        using var context = TestSupport.CreateContext();
        var genre = new GenreEntity { Name = "Adventure" };
        context.Genres.AddRange(genre, new GenreEntity { Name = "Action" });
        await context.SaveChangesAsync();
        var handler = new UpdateGenreCommandHandler(context, new TestCurrentUser());

        await Assert.ThrowsAsync<MarketConflictException>(() => handler.Handle(
            new UpdateGenreCommand { Id = genre.Id, Name = "ACTION" }, CancellationToken.None));
        context.ChangeTracker.Clear();
        Assert.Equal("Adventure", (await context.Genres.SingleAsync(g => g.Id == genre.Id)).Name);
    }

    [Fact]
    public async Task Update_WhenMissing_ShouldThrowNotFound()
    {
        using var context = TestSupport.CreateContext();
        var handler = new UpdateGenreCommandHandler(context, new TestCurrentUser());
        await Assert.ThrowsAsync<MarketNotFoundException>(() => handler.Handle(
            new UpdateGenreCommand { Id = 999, Name = "Action" }, CancellationToken.None));
    }

    [Fact]
    public async Task Delete_ShouldSoftDeleteGenre()
    {
        using var context = TestSupport.CreateContext();
        var genre = new GenreEntity { Name = "Adventure" };
        context.Genres.Add(genre);
        await context.SaveChangesAsync();
        var handler = new DeleteGenreCommandHandler(context, new TestCurrentUser());

        await handler.Handle(new DeleteGenreCommand { Id = genre.Id }, CancellationToken.None);

        context.ChangeTracker.Clear();
        Assert.Empty(await context.Genres.ToListAsync());
        Assert.True((await context.Genres.IgnoreQueryFilters().SingleAsync()).IsDeleted);
    }

    [Fact]
    public async Task Delete_WhenAssignedToGame_ShouldKeepGenre()
    {
        using var context = TestSupport.CreateContext();
        var genre = new GenreEntity { Name = "Adventure" };
        context.GameGenres.Add(new GameGenreEntity { Genre = genre, Game = TestSupport.Game() });
        await context.SaveChangesAsync();
        var handler = new DeleteGenreCommandHandler(context, new TestCurrentUser());

        await Assert.ThrowsAsync<MarketBusinessRuleException>(() => handler.Handle(
            new DeleteGenreCommand { Id = genre.Id }, CancellationToken.None));
        Assert.True(await context.Genres.AnyAsync(g => g.Id == genre.Id));
    }

    [Fact]
    public async Task List_ShouldTrimSearchAndIgnoreCase()
    {
        using var context = TestSupport.CreateContext();
        context.Genres.AddRange(new GenreEntity { Name = "Adventure" }, new GenreEntity { Name = "Action" });
        await context.SaveChangesAsync();
        var handler = new ListGenresQueryHandler(context, new TestCurrentUser());

        var result = await handler.Handle(new ListGenresQuery { Search = "  VENT  " }, CancellationToken.None);

        Assert.Equal(1, result.Total);
        Assert.Equal("Adventure", Assert.Single(result.Items).Name);
    }
}
