using Market.Domain.Entities;
using Market.Domain.Entities.Catalog;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Payments.Commands.ProcessStripeWebhook
{
    public sealed class ProcessStripeWebhookCommandHandler(IAppDbContext context)
        :IRequestHandler<ProcessStripeWebhookCommand, Unit>
    {
        public async Task<Unit> Handle(ProcessStripeWebhookCommand request, CancellationToken ct)
        {
            var alreadyProcessed = await context.ProcessedWebhookEvents.AnyAsync(x => x.Provider == "Stripe" && x.EventId == request.EventId, ct);

            if (alreadyProcessed)
                return Unit.Value;


            context.ProcessedWebhookEvents.Add(new ProcessedWebhookEventEntity
            {
                EventId = request.EventId,
                Provider = "Stripe",
                EventType = request.EventType,
                ReceivedAtUtc = DateTime.UtcNow,
            });

            var isCompleted = request.EventType == "checkout.session.completed";
            var isExpired = request.EventType == "checkout.session.expired";


            if (!isCompleted && !isExpired)
            {
                await context.SaveChangesAsync(ct);
                return Unit.Value;
            }


            var order = request.OrderId.HasValue ? await context.Orders.Include(o => o.Payment).FirstOrDefaultAsync(o => o.Id == request.OrderId.Value, ct) : null;

            if (order is null && !string.IsNullOrWhiteSpace(request.SessionId))
            {
                order = await context.Orders
                    .Include(o => o.Payment)
                    .FirstOrDefaultAsync(o => o.Payment != null && o.Payment.StripeCheckoutSessionId == request.SessionId, ct);
            }
            if (order is null)
            {   
                await context.SaveChangesAsync(ct);
                return Unit.Value;
            }


            if (isExpired)
            {
                if (order.OrderStatus == "Pending")
                {
                    order.OrderStatus = "Cancelled";
                }

                await context.SaveChangesAsync(ct);
                return Unit.Value;
            }




            if (order.OrderStatus == "Paid")
            {
                await context.SaveChangesAsync(ct);
                return Unit.Value;
            }

            
            if (order.OrderStatus != "Pending") 
            { 
                await context.SaveChangesAsync(ct); 
                return Unit.Value; 
            }

            if(order.Payment is null)
            {
                order.Payment = new PaymentEntity
                {
                    OrderId = order.Id,
                    Total = order.TotalAmount,
                    PaymentStatus = "Pending"
                };
                context.Payments.Add(order.Payment);
            }

            order.OrderStatus = "Paid";
            order.Payment.PaymentStatus = "Succeeded";
            order.Payment.StripeCheckoutSessionId= request.SessionId ?? order.Payment.StripeCheckoutSessionId;
            order.Payment.StripePaymentIntentId= request.PaymentIntentId?? order.Payment.StripePaymentIntentId;
            order.Payment.Total = order.TotalAmount;




            //add games to usergames

            var userId = order.UserId;
            var gamesToBuy = await context.OrderItems
                .Where(oi => oi.OrderId == order.Id)
                .Select(oi=>oi.GameId)
                .Distinct()
                .ToListAsync(ct);

            if (gamesToBuy.Count > 0)
            {
                var alreadyOwnedGames = await context.UserGames
                    .Where(ug => ug.UserId == userId && gamesToBuy.Contains(ug.GameId))
                    .Select(ug => ug.GameId)
                    .ToListAsync(ct);


                foreach(var gameId in gamesToBuy.Except(alreadyOwnedGames))
                {
                    context.UserGames.Add(new UserGameEntity
                    {
                        UserId = userId,
                        GameId = gameId,
                        PurchaseDate = DateTime.UtcNow,
                    });
                }
            }




            // clear cart

            var cart = await context.Carts
                .Include(c=>c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId,ct);

            if(cart.CartItems !=null && cart.CartItems.Count > 0)
            {
                context.CartItems.RemoveRange(cart.CartItems);
                cart.TotalPrice = 0m;
            }



            await context.SaveChangesAsync(ct);
            return Unit.Value;


        }



    }
}
