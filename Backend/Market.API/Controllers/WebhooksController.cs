using Market.Application.Abstractions;
using Market.Application.Modules.Payments.Commands.ProcessStripeWebhook;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/webhooks")]
    public sealed class WebhooksController(
        ISender sender,
        IOptions<StripeOptions> stripeOptions,
        ILogger<WebhooksController> logger) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("stripe")]
        public async Task<IActionResult> Stripe(CancellationToken ct)
        {
            Request.EnableBuffering();
            Request.Body.Position = 0;
            using var reader = new StreamReader(Request.Body, System.Text.Encoding.UTF8, leaveOpen: true);
            var json = await reader.ReadToEndAsync(ct);
            Request.Body.Position = 0;

            var signatureHeader = Request.Headers["Stripe-Signature"].ToString();
            if (string.IsNullOrWhiteSpace(signatureHeader))
            {
                logger.LogWarning("Stripe webhook received without Stripe-Signature header.");
                return BadRequest("Missing Stripe-Signature header.");
            }

            Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    signatureHeader,
                    stripeOptions.Value.WebhookSecret
                );
            }
            catch (StripeException ex)
            {
                logger.LogError(ex, "Stripe webhook signature validation failed. Please ensure Stripe:WebhookSecret in appsettings.json matches the secret from Stripe CLI or dashboard.");
                return BadRequest($"Signature verification failed: {ex.Message}");
            }

            logger.LogInformation("Received verified Stripe event: {Type} ({Id})", stripeEvent.Type, stripeEvent.Id);

            string? sessionId = null;
            string? paymentIntentId = null;
            int? orderId = null;

            if (stripeEvent.Type == "checkout.session.completed" ||
                stripeEvent.Type == "checkout.session.expired")
            {
                var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

                sessionId = session?.Id;
                paymentIntentId = session?.PaymentIntentId;

                if (session?.Metadata != null &&
                    session.Metadata.TryGetValue("orderId", out var orderIdStr) &&
                    int.TryParse(orderIdStr, out var parsedOrderId))
                {
                    orderId = parsedOrderId;
                }
                else if (int.TryParse(session?.ClientReferenceId, out var parsedFromClientRef))
                {
                    orderId = parsedFromClientRef;
                }
            }

            await sender.Send(new ProcessStripeWebhookCommand(
                stripeEvent.Id,
                stripeEvent.Type,
                sessionId,
                paymentIntentId,
                orderId
            ), ct);

            return Ok();
        }
    }
}
