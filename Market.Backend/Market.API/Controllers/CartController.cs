using Market.Application.Modules.Carts.Commands.Create;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CartController(ISender sender) : ControllerBase
    {
        [HttpPost("AddToCart")]
        public async Task<ActionResult> Add(AddToCartCommand command, CancellationToken ct)
        {
            await sender.Send(command, ct);
            return Ok();
        }
    }
}
