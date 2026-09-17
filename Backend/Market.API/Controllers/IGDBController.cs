using Market.Application.Modules.IGDB.Queries.GetIGDBGameDetails;
using Market.Application.Modules.IGDB.Queries.SearchIGDBGames;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/igdb")]
    [Authorize]
    public class IGDBController(ISender sender) : ControllerBase
    {
        [HttpGet("search")]
        public async Task<List<SearchIGDBGamesQueryDto>> SearchIGDBGames([FromQuery] SearchIGDBGamesQuery query, CancellationToken ct)
        {
            return await sender.Send(query, ct);
        }

        [HttpGet("{id:int}")]
        public async Task<GetIGDBGameDetailsDto> GetGameDetails(int id, CancellationToken ct)
        {
            return await sender.Send(new GetIGDBGameDetailsQuery { GameId = id }, ct);
        }

    }
}
