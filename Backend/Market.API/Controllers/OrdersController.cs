using Market.Application.Modules.Orders.Commands.Create;
using Market.Application.Modules.Orders.Commands.Delete;
using Market.Application.Modules.Orders.Queries.List;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/orders")]

    public class OrdersController(ISender sender): ControllerBase
    {
        [HttpGet]
        public async Task<PageResult<ListOrdersQueryDto>> List([FromQuery] ListOrdersQuery query, CancellationToken ct)
        {
            return await sender.Send(query, ct);
        }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult<int>> Create(CancellationToken ct)
        {
            int orderId = await sender.Send(new CreateOrderCommand(), ct);
            return Ok(orderId);
        }

        [HttpPut("CancelOrder/{id:int}")]
        public async Task<ActionResult> Cancel(int id, CancellationToken ct)
        {
            await sender.Send(new CancelOrderCommand { OrderId = id }, ct);
            return NoContent();
        }
    }
}
