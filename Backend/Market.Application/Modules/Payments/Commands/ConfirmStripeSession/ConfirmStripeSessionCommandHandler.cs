using Market.Application.Abstractions;
using Stripe.Checkout;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Application.Modules.Payments.Commands.ConfirmStripeSession
{
    public sealed class ConfirmStripeSessionCommandHandler(
        IOrderFulfillmentService fulfillmentService)
        : IRequestHandler<ConfirmStripeSessionCommand, ConfirmStripeSessionResponse>
    {
        public async Task<ConfirmStripeSessionResponse> Handle(ConfirmStripeSessionCommand request, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(request.SessionId))
                return new ConfirmStripeSessionResponse(false, null, "SessionId cannot be empty.");

            try
            {
                var session = await new SessionService().GetAsync(request.SessionId, cancellationToken: ct);

                if (session is null)
                    return new ConfirmStripeSessionResponse(false, null, "Stripe session not found.");

                if (session.PaymentStatus != "paid" && session.Status != "complete")
                    return new ConfirmStripeSessionResponse(false, null, $"Payment incomplete. Status: {session.PaymentStatus}");

                int? orderId = session.Metadata?.TryGetValue("orderId", out var idStr) == true && int.TryParse(idStr, out var id) ? id
                             : int.TryParse(session.ClientReferenceId, out var refId) ? refId : null;

                var success = await fulfillmentService.FulfillOrderAsync(orderId, session.Id, session.PaymentIntentId, ct);

                return new ConfirmStripeSessionResponse(success, orderId, success ? "Order confirmed successfully." : "Order could not be fulfilled.");
            }
            catch (Exception ex)
            {
                return new ConfirmStripeSessionResponse(false, null, $"Error confirming session: {ex.Message}");
            }
        }
    }
}