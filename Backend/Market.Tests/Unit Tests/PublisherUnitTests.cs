using Market.Application.Common.Exceptions;
using Market.Application.Modules.Publishers.Commands.Create;
using Market.Application.Modules.Publishers.Commands.Delete;
using Market.Application.Modules.Publishers.Commands.Update;
using Market.Application.Modules.Publishers.Queries.GetById;
using Market.Domain.Entities;

namespace Market.Tests.UnitTests;

public class PublisherUnitTests
{
    [Fact]
    public async Task Create_ShouldPersistPublisherAndCountry()
    {
        // Arrange
        using var context = TestSupport.CreateContext();
        var country = new CountryEntity { Name = "Test Country" };
        context.Countries.Add(country);
        await context.SaveChangesAsync();
        var handler = new CreatePublisherCommandHandler(new TestCurrentUser(), context);

        // Act
        var id = await handler.Handle(new CreatePublisherCommand { Name = "New Publisher", CountryId = country.Id }, CancellationToken.None);

        // Assert
        context.ChangeTracker.Clear();
        var publisher = await context.Publishers.SingleAsync(p => p.Id == id);
        Assert.Equal("New Publisher", publisher.Name);
        Assert.Equal(country.Id, publisher.CountryId);
    }

    [Fact]
    public async Task Create_WhenCountryMissing_ShouldNotSave()
    {
        using var context = TestSupport.CreateContext();
        var handler = new CreatePublisherCommandHandler(new TestCurrentUser(), context);
        await Assert.ThrowsAsync<MarketNotFoundException>(() => handler.Handle(
            new CreatePublisherCommand { Name = "New Publisher", CountryId = 999 }, CancellationToken.None));
        Assert.Empty(await context.Publishers.ToListAsync());
    }

    [Fact]
    public async Task Create_WhenNameExists_ShouldRejectDuplicate()
    {
        using var context = TestSupport.CreateContext();
        var publisher = TestSupport.Publisher();
        context.Publishers.Add(publisher);
        await context.SaveChangesAsync();
        var handler = new CreatePublisherCommandHandler(new TestCurrentUser(), context);
        await Assert.ThrowsAsync<MarketConflictException>(() => handler.Handle(
            new CreatePublisherCommand { Name = publisher.Name, CountryId = publisher.CountryId }, CancellationToken.None));
        Assert.Equal(1, await context.Publishers.CountAsync());
    }

    [Fact]
    public async Task Create_WhenNotAdmin_ShouldNotSave()
    {
        using var context = TestSupport.CreateContext();
        var handler = new CreatePublisherCommandHandler(new TestCurrentUser { IsAdmin = false }, context);
        await Assert.ThrowsAsync<MarketForbiddenException>(() => handler.Handle(
            new CreatePublisherCommand { Name = "New Publisher", CountryId = 1 }, CancellationToken.None));
        Assert.Empty(await context.Publishers.ToListAsync());
    }

    [Fact]
    public async Task Update_ShouldChangeNameAndCountry()
    {
        using var context = TestSupport.CreateContext();
        var publisher = TestSupport.Publisher();
        var country = new CountryEntity { Name = "New Country" };
        context.AddRange(publisher, country);
        await context.SaveChangesAsync();
        var handler = new UpdatePublisherCommandHandler(context, new TestCurrentUser());

        await handler.Handle(new UpdatePublisherCommand { Id = publisher.Id, Name = "Updated", CountryId = country.Id }, CancellationToken.None);

        context.ChangeTracker.Clear();
        var saved = await context.Publishers.SingleAsync();
        Assert.Equal("Updated", saved.Name);
        Assert.Equal(country.Id, saved.CountryId);
    }

    [Fact]
    public async Task Update_WhenCountryMissing_ShouldKeepOriginalValues()
    {
        using var context = TestSupport.CreateContext();
        var publisher = TestSupport.Publisher();
        context.Publishers.Add(publisher);
        await context.SaveChangesAsync();
        var handler = new UpdatePublisherCommandHandler(context, new TestCurrentUser());

        await Assert.ThrowsAsync<MarketNotFoundException>(() => handler.Handle(
            new UpdatePublisherCommand { Id = publisher.Id, Name = "Updated", CountryId = 999 }, CancellationToken.None));
        context.ChangeTracker.Clear();
        var saved = await context.Publishers.SingleAsync();
        Assert.Equal(publisher.Name, saved.Name);
        Assert.Equal(publisher.CountryId, saved.CountryId);
    }

    [Fact]
    public async Task Delete_ShouldSoftDeletePublisher()
    {
        using var context = TestSupport.CreateContext();
        var publisher = TestSupport.Publisher();
        context.Publishers.Add(publisher);
        await context.SaveChangesAsync();
        var handler = new DeletePublisherCommandHandler(new TestCurrentUser(), context);

        await handler.Handle(new DeletePublisherCommand { Id = publisher.Id }, CancellationToken.None);

        context.ChangeTracker.Clear();
        Assert.Empty(await context.Publishers.ToListAsync());
        Assert.True((await context.Publishers.IgnoreQueryFilters().SingleAsync()).IsDeleted);
    }

    [Fact]
    public async Task Delete_WhenGamesExist_ShouldKeepPublisher()
    {
        using var context = TestSupport.CreateContext();
        var game = TestSupport.Game();
        context.Games.Add(game);
        await context.SaveChangesAsync();
        var handler = new DeletePublisherCommandHandler(new TestCurrentUser(), context);

        await Assert.ThrowsAsync<MarketBusinessRuleException>(() => handler.Handle(
            new DeletePublisherCommand { Id = game.PublisherId }, CancellationToken.None));
        Assert.True(await context.Publishers.AnyAsync(p => p.Id == game.PublisherId));
    }

    [Fact]
    public async Task GetById_ShouldReturnCountryAndGames()
    {
        using var context = TestSupport.CreateContext();
        var game = TestSupport.Game();
        context.Games.Add(game);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
        var handler = new GetPublisherByIdQueryHandle(new TestCurrentUser(), context);

        var result = await handler.Handle(new GetPublisherByIdQuery { Id = game.PublisherId }, CancellationToken.None);

        Assert.Equal(game.Publisher.Name, result.Name);
        Assert.Equal("Test Country", result.Country.Name);
        Assert.Equal(game.Id, Assert.Single(result.Games).Id);
    }

    [Fact]
    public async Task GetById_WhenMissing_ShouldThrowNotFound()
    {
        using var context = TestSupport.CreateContext();
        var handler = new GetPublisherByIdQueryHandle(new TestCurrentUser(), context);
        await Assert.ThrowsAsync<MarketNotFoundException>(() => handler.Handle(
            new GetPublisherByIdQuery { Id = 999 }, CancellationToken.None));
    }
}
