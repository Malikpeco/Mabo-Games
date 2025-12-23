using Market.Application.Modules.Games.Dto;
using Market.Application.Modules.Games.Queries.GetGameDetails;
using Market.Application.Modules.Games.Queries.GetStorefrontGames;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/games")]
    public sealed class GamesController(ISender sender) : ControllerBase
    {
        [HttpGet("storefront")]
        public async Task<PageResult<StorefrontGameDto>> GetStorefront([FromQuery] GetStorefrontGamesQuery query, CancellationToken ct)
        {
            return await sender.Send(query, ct);
        }

        [HttpGet("{id:int}")]
        public async Task<GameDetailsDto> GetGameDetails([FromRoute] int id, CancellationToken ct)
        {
            return await sender.Send(new GetGameDetailsQuery { Id = id }, ct);
        }

    }

    
}
