using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Issuing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Carts.Commands.SwitchItemState
{
    public class SwitchItemStateCommandHandler(IAppDbContext context)
        : IRequestHandler<SwitchItemStateCommand, Unit>
    {
        public async Task<Unit> Handle(SwitchItemStateCommand request, CancellationToken cancellationToken)
        {
            await context.Games.ToListAsync(cancellationToken);
            var item = await context.CartItems.FirstOrDefaultAsync(ci => ci.Id == request.CartItemId, cancellationToken);
            var cart = await context.Carts.FirstOrDefaultAsync(c => item.CartId == c.Id,cancellationToken);


            item.IsSaved = !item.IsSaved;
            context.CartItems.Update(item);
            await context.SaveChangesAsync(cancellationToken);


            var total = await context.CartItems
            .Where(ci => ci.CartId == cart.Id && !ci.IsSaved)
            .Join(context.Games,
                ci => ci.GameId,
                g => g.Id,
                (ci, g) => g.Price)
            .SumAsync(cancellationToken);

            cart.TotalPrice = total;
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;

        }
    }
}
