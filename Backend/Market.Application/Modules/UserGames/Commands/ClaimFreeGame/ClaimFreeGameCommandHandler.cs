using Market.Domain.Entities;

namespace Market.Application.Modules.UserGames.Commands.ClaimFreeGame
{
    public sealed class ClaimFreeGameCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<ClaimFreeGameCommand, int>
    {
        public async Task<int> Handle(ClaimFreeGameCommand request, CancellationToken ct)
        {
            if (currentUser.UserId is null)
                throw new MarketNotFoundException("User not found.");

            var userId = currentUser.UserId.Value;

            var game = await context.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == request.GameId, ct);

            if (game is null)
                throw new MarketNotFoundException("Game not found.");

            if (game.Price > 0)
                throw new MarketBusinessRuleException("409", "Only free games can be added directly to the library.");

            var alreadyOwned = await context.UserGames
                .AnyAsync(ug => ug.UserId == userId && ug.GameId == request.GameId, ct);

            if (alreadyOwned)
                throw new MarketBusinessRuleException("409", "You already own this game.");

            var userGame = new UserGameEntity
            {
                UserId = userId,
                GameId = request.GameId,
                PurchaseDate = DateTime.UtcNow,
            };

            context.UserGames.Add(userGame);

            await context.SaveChangesAsync(ct);
            return userGame.Id;
        }
    }
}