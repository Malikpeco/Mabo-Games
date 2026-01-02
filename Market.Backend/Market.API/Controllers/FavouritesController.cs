using Market.Application.Modules.Favourites.Commands.Create;
using Market.Application.Modules.Favourites.Commands.Delete;
using Market.Application.Modules.Favourites.Queries.List;
using Market.Application.Modules.Games.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/favourites")]
    public sealed class FavouritesController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateFavouriteCommand command, CancellationToken ct)
        {
            await sender.Send(command, ct);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id, CancellationToken ct)
        {
            await sender.Send(new DeleteFavouriteCommand { GameId = id }, ct);
            return Ok();
        }
        
        [HttpGet]
        public async Task<PageResult<StorefrontGameDto>> List([FromQuery]ListFavouritesQuery query, CancellationToken ct)
        {
            return await sender.Send(query, ct);
        }
    }
}
