using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Carts.Commands.Delete
{
    public sealed class RemoveFromCartCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<RemoveFromCartCommand,Unit>
    {
        public async Task<Unit> Handle(RemoveFromCartCommand request, CancellationToken ct)
        {
            var cart = await context.Carts
                .Include(c=>c.CartItems)
                .ThenInclude(ci=>ci.Game)
                .FirstOrDefaultAsync(c => c.UserId == currentUser.UserId,ct);

            var cartitem = cart.CartItems.FirstOrDefault(ci => ci.GameId == request.GameId);
            if (cartitem == null)
            {
                throw new MarketNotFoundException(
                $"Game with id {request.GameId} is not in your cart."
                );
            }

            context.CartItems.Remove(cartitem);
            await context.SaveChangesAsync(ct);

            cart.TotalPrice -= cartitem.Game.Price;
            await context.SaveChangesAsync(ct);
            return Unit.Value;

        }
    }
}
