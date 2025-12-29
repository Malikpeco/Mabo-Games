using Market.Application.Modules.Payments.Commands.CreateStripeCheckoutSession;
using Market.Application.Modules.Payments.Dto;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/payments")]

    public sealed class PaymentsController(ISender sender) :ControllerBase
    {
        [HttpPost("stripe/checkout-session")]
        public async Task<ActionResult<CreateStripeCheckoutSessionResponse>> CreateStripeCheckoutSession([FromBody] CreateStripeCheckoutSessionRequest request, CancellationToken ct)
        {
            var result = await sender.Send(new CreateStripeCheckoutSessionCommand(request.OrderId), ct);
            return Ok(result);
        }
    }
}
