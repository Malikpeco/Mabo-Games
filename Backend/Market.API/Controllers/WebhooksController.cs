using Market.Application.Abstractions;
using Market.Application.Modules.Payments.Commands.ProcessStripeWebhook;
using Microsoft.Extensions.Options;
using Stripe;

namespace Market.API.Controllers
{
    
    [ApiController]
    [Route("api/webhooks")]
    public sealed class WebhooksController(ISender sender, IOptions<StripeOptions> stripeOptions) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("stripe")]
        public async Task<IActionResult> Stripe(CancellationToken ct)
        {
            
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync(ct);

            
            var signatureHeader = Request.Headers["Stripe-Signature"].ToString();
            if (string.IsNullOrWhiteSpace(signatureHeader))
                return BadRequest();

            Event stripeEvent;

            
            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    signatureHeader,
                    stripeOptions.Value.WebhookSecret
                );
            }
            catch (StripeException)
            {
                return BadRequest();
            }


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
