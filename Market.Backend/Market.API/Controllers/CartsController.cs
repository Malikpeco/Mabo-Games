using Market.Application.Modules.Carts.Commands.Create;
using Market.Application.Modules.Carts.Commands.Delete;
using Market.Application.Modules.Carts.Commands.DeleteUnsavedCartItems;
using Market.Application.Modules.Carts.Dto;
using Market.Application.Modules.Carts.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CartsController(ISender sender) : ControllerBase
    {
        [HttpPost("AddToCart")]
        public async Task<ActionResult> Add(AddToCartCommand command, CancellationToken ct)
        {
            await sender.Send(command, ct);
            return Ok();
        }



        [HttpGet("GetCart")]
        public async Task<CartDto> Get(CancellationToken ct)
        {
            return await sender.Send(new GetCartByUserIdQuery(), ct);
        }



        [HttpDelete("RemoveFromCart/{gameId:int}")]
        public async Task<ActionResult> Remove(int gameId, CancellationToken ct)
        {
            await sender.Send(new RemoveFromCartCommand { GameId = gameId }, ct);
            return NoContent();
        }



        //TODO:
        //When frontend is added, call this endpoint on user login, immediately after logging in, call this, to clear his cart, and leave only items with isSaved==true
        [HttpPost("CartCleanup")]
        public async Task<ActionResult> Cleanup(CancellationToken ct)
        {
            await sender.Send(new CartCleanupCommand(), ct);
            return Ok();
        }



    }
}
