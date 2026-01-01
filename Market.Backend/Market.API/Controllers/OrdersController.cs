using Market.Application.Modules.Orders.Commands.Create;
using Market.Application.Modules.Orders.Commands.Delete;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/orders")]

    public class OrdersController(ISender sender): ControllerBase
    {
        [HttpPost("CreateOrder/BeginCheckout")]
        public async Task<ActionResult<int>> Create(CancellationToken ct)
        {
            int orderId = await sender.Send(new CreateOrderCommand(), ct);
            return Ok(orderId);
        }

        [HttpPut("CancelOrder")]
        public async Task<ActionResult> Cancel(int id, CancellationToken ct)
        {
            await sender.Send(new CancelOrderCommand { OrderId = id }, ct);
            return NoContent();
        }
    }
}
