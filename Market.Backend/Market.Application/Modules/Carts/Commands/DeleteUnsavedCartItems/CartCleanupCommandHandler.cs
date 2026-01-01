using Stripe.Tax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Carts.Commands.DeleteUnsavedCartItems
{
    public sealed class CartCleanupCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<CartCleanupCommand, Unit>
    {
        public async Task<Unit> Handle(CartCleanupCommand request, CancellationToken ct)
        {
            var userId = currentUser.UserId;
            var cart = await context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Game)
                .FirstOrDefaultAsync(c => c.UserId == userId, ct);

            if (cart is null)
                return Unit.Value;

            var toRemove = cart.CartItems
                .Where(ci => ci.IsSaved == false)
                .ToList();

            if (toRemove.Count == 0)
                return Unit.Value;

            cart.TotalPrice = cart.CartItems
                .Where(ci => ci.IsSaved)
                .Sum(ci => ci.Game.Price);

            context.CartItems.RemoveRange(toRemove);
            await context.SaveChangesAsync(ct);

            return Unit.Value;

        }
    }
}