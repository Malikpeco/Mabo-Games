using Market.Application.Abstractions;
using Market.Application.Common.Achievements;
using Market.Domain.Entities;
using Market.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Application.Modules.Payments.Services
{
    public sealed class OrderFulfillmentService(
        IAppDbContext context,
        IAchievementSystem achievementSystem) : IOrderFulfillmentService
    {
        public async Task<bool> FulfillOrderAsync(int? orderId, string? sessionId, string? paymentIntentId, CancellationToken ct = default)
        {
            var order = orderId.HasValue
                ? await context.Orders.Include(o => o.Payment).FirstOrDefaultAsync(o => o.Id == orderId.Value, ct)
                : null;

            if (order is null && !string.IsNullOrWhiteSpace(sessionId))
            {
                order = await context.Orders
                    .Include(o => o.Payment)
                    .FirstOrDefaultAsync(o => o.Payment != null && o.Payment.StripeCheckoutSessionId == sessionId, ct);
            }

            if (order is null)
            {
                return false;
            }

            if (order.OrderStatus == "Paid")
            {
                return true;
            }

            if (order.Payment is null)
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
            order.Payment.StripeCheckoutSessionId = sessionId ?? order.Payment.StripeCheckoutSessionId;
            order.Payment.StripePaymentIntentId = paymentIntentId ?? order.Payment.StripePaymentIntentId;
            order.Payment.Total = order.TotalAmount;

            // Add purchased games to user's library
            var userId = order.UserId;
            var gamesToBuy = await context.OrderItems
                .Where(oi => oi.OrderId == order.Id)
                .Select(oi => oi.GameId)
                .Distinct()
                .ToListAsync(ct);

            if (gamesToBuy.Count > 0)
            {
                var alreadyOwnedGames = await context.UserGames
                    .Where(ug => ug.UserId == userId && gamesToBuy.Contains(ug.GameId))
                    .Select(ug => ug.GameId)
                    .ToListAsync(ct);

                var newGameIds = gamesToBuy.Except(alreadyOwnedGames).ToList();
                foreach (var gameId in newGameIds)
                {
                    context.UserGames.Add(new UserGameEntity
                    {
                        UserId = userId,
                        GameId = gameId,
                        PurchaseDate = DateTime.UtcNow,
                    });
                }
            }

            // Clear purchased/non-saved items from cart
            var cart = await context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId, ct);

            if (cart != null && cart.CartItems != null)
            {
                var toRemove = cart.CartItems.Where(ci => !ci.IsSaved).ToList();
                if (toRemove.Count > 0)
                {
                    context.CartItems.RemoveRange(toRemove);
                    cart.TotalPrice = 0m;
                }
            }

            await context.SaveChangesAsync(ct);

            try
            {
                await achievementSystem.CheckEligibilityAsync(userId, AchievementTriggerType.GamePurchased, ct);
            }
            catch (Exception)
            {
                
            }

            return true;
        }
    }
}