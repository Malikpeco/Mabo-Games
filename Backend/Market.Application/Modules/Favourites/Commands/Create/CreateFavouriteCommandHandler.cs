using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Favourites.Commands.Create
{
    public sealed class CreateFavouriteCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
        :IRequestHandler<CreateFavouriteCommand, Unit>
    {
        public async Task<Unit> Handle(CreateFavouriteCommand request, CancellationToken ct)
        {
            if (currentUser.UserId is null)
                throw new Exception("User not found.");

            var userId = currentUser.UserId.Value;

            if (!await context.Users.AnyAsync(u => u.Id == userId, ct))
                throw new MarketNotFoundException("User not found");
            if (!await context.Games.AnyAsync(g => g.Id == request.GameId, ct))
                throw new MarketNotFoundException("Game not found");

            if (await context.Favourites.AnyAsync(f => f.UserId == userId && f.GameId == request.GameId, ct))
                throw new MarketBusinessRuleException("422", "Favourite already exists.");

            var fav = new FavouriteEntity
            {
                UserId = userId,
                GameId = request.GameId,
            };

            context.Favourites.Add(fav);
            await context.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}
