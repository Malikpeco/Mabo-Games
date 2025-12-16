using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Orders.Commands.Create
{
    public sealed class CreateOrderCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
        :IRequestHandler<CreateOrderCommand, int>
    {
        public async Task<int> Handle(CreateOrderCommand request, CancellationToken ct)
        {


            //THIS MAKES IT SO THAT A USER CAN ONLY HAVE 1 PENDING ORDER AT A TIME, IF ANOTHER ORDER IS CREATED, THE OLD PENDING ORDER BECOMES CANCELLED
            var pendingOrders = await context.Orders
                .Where(o => o.UserId == currentUser.UserId && o.OrderStatus == "Pending")
                .ToListAsync(ct);

            foreach (var po in pendingOrders)
            {
                po.OrderStatus = "Cancelled";
            }



            var cart = await context.Carts
                .Include(c=>c.CartItems).ThenInclude(ci=>ci.Game)
                .FirstOrDefaultAsync(c => c.UserId == currentUser.UserId, ct);

            if (cart.CartItems == null || cart.CartItems.Count == 0)
                throw new Exception("Cart is empty!");

            var order = new OrderEntity
            {
                UserId = currentUser.UserId.Value,
                TotalAmount = 0.0m,
                OrderStatus = "Pending"
            };
            context.Orders.Add(order);

            foreach (var ci in cart.CartItems)
            {
                var oi = new OrderItemEntity
                {
                    Order = order,
                    GameId = ci.GameId,
                    Price = ci.Game.Price,
                };
                context.OrderItems.Add(oi);
                order.TotalAmount += oi.Price;

            }

            await context.SaveChangesAsync(ct);
            return order.Id;
        }
    }
}
