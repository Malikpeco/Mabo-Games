using Market.Application.Modules.UserGames.Commands.ClaimFreeGame;
using Market.Application.Modules.Games.Dto;
using Market.Application.Modules.UserGames.Queries.List;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/usergames")]
    public sealed class UserGamesController(ISender sender) : ControllerBase
    {
        [HttpPost("claim-free")]
        public async Task<ActionResult<int>> ClaimFree([FromBody] ClaimFreeGameCommand command, CancellationToken ct)
        {
            var userGameId = await sender.Send(command, ct);
            return Ok(userGameId);
        }

        [HttpGet]
        public async Task<PageResult<ListUserGamesQueryDto>> List([FromQuery]ListUserGamesQuery query, CancellationToken ct)
        {
            return await sender.Send(query, ct);
        }
    }
}
