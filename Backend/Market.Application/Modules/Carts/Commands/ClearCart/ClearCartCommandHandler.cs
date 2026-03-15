using Stripe.Tax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Carts.Commands.ClearCart
{
    public sealed class CleanCartCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<ClearCartCommand, Unit>
    {
        public async Task<Unit> Handle(ClearCartCommand request, CancellationToken ct)
        {
            var userId = currentUser.UserId;

            await CancelPendingOrders(context, userId.Value, ct);

            var cart = await context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Game)
                .FirstOrDefaultAsync(c => c.UserId == userId, ct);

            if (cart is null)
                return Unit.Value;

            var toRemove = cart.CartItems.Where(ci=>!ci.IsSaved)
                .ToList();

            if (toRemove.Count == 0)
                return Unit.Value;

            cart.TotalPrice = 0;

            context.CartItems.RemoveRange(toRemove);
            await context.SaveChangesAsync(ct);

            return Unit.Value;

        }

        private static async Task CancelPendingOrders(IAppDbContext context, int userId, CancellationToken ct)
        {
            var pendingOrders = await context.Orders
                .Where(o => o.UserId == userId && o.OrderStatus == "Pending")
                .ToListAsync(ct);

            if (pendingOrders.Count == 0)
                return;

            foreach (var order in pendingOrders)
            {
                order.OrderStatus = "Cancelled";
            }

            await context.SaveChangesAsync(ct);
        }
    }
}