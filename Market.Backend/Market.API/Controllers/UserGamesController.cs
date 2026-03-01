using Market.Application.Modules.Games.Dto;
using Market.Application.Modules.UserGames.Queries.List;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/usergames")]
    public sealed class UserGamesController(ISender sender)
    {
        [HttpGet]
        public async Task<PageResult<ListUserGamesQueryDto>> List([FromQuery]ListUserGamesQuery query, CancellationToken ct)
        {
            return await sender.Send(query, ct);
        }
    }
}
