using Market.Application.Abstractions;
using Market.Domain.Entities;
using Market.Domain.Entities.Catalog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Application.Modules.Payments.Commands.ProcessStripeWebhook
{
    public sealed class ProcessStripeWebhookCommandHandler(
        IAppDbContext context,
        IOrderFulfillmentService fulfillmentService)
        : IRequestHandler<ProcessStripeWebhookCommand, Unit>
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

            if (isExpired)
            {
                var order = request.OrderId.HasValue 
                    ? await context.Orders.FirstOrDefaultAsync(o => o.Id == request.OrderId.Value, ct) 
                    : null;

                if (order is null && !string.IsNullOrWhiteSpace(request.SessionId))
                {
                    order = await context.Orders
                        .Include(o => o.Payment)
                        .FirstOrDefaultAsync(o => o.Payment != null && o.Payment.StripeCheckoutSessionId == request.SessionId, ct);
                }

                if (order != null && order.OrderStatus == "Pending")
                {
                    order.OrderStatus = "Cancelled";
                }

                await context.SaveChangesAsync(ct);
                return Unit.Value;
            }

            await context.SaveChangesAsync(ct);
            await fulfillmentService.FulfillOrderAsync(request.OrderId, request.SessionId, request.PaymentIntentId, ct);

            return Unit.Value;
        }
    }
}

