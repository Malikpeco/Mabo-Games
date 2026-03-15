using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Favourites.Commands.Delete
{
    public sealed class DeleteFavouriteCommandHandler(IAppDbContext context) 
        :IRequestHandler<DeleteFavouriteCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteFavouriteCommand request, CancellationToken ct)
        {
            var fav = await context.Favourites.FirstOrDefaultAsync(f => f.GameId == request.GameId, ct);
            if (fav is null)
                throw new MarketNotFoundException("Favourite not found.");

            context.Favourites.Remove(fav);

            await context.SaveChangesAsync(ct);
            return Unit.Value;

        }
    }
}
