using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Orders.Commands.Delete
{
    public class CancelOrderCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<CancelOrderCommand, Unit>
    {
        public async Task<Unit> Handle(CancelOrderCommand request, CancellationToken ct)
        {
            int userId = currentUser.UserId.Value;

            var order = await context.Orders.FirstOrDefaultAsync(o => o.Id==request.OrderId && o.UserId == userId, ct);

            if (order == null)
            {
                throw new ValidationException("Order not found");
            }

            if (order.OrderStatus != "Pending")
            {
                return Unit.Value;//order is already Paid or Cancelled.
            }

            order.OrderStatus = "Cancelled";

            await context.SaveChangesAsync(ct);
            return Unit.Value;

        }

    }
}
