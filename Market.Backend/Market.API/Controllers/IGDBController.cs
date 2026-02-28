using Market.Application.Modules.IGDB.Queries.GetIGDBGameDetails;
using Market.Application.Modules.IGDB.Queries.SearchIGDBGames;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/igdb")]
    public class IGDBController(ISender sender) : ControllerBase
    {
        //Note to self, remove the allow anon

        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<List<SearchIGDBGamesQueryDto>> SearchIGDBGames([FromQuery] SearchIGDBGamesQuery query, CancellationToken ct)
        {
            return await sender.Send(query, ct);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<GetIGDBGameDetailsDto> GetGameDetails(int id, CancellationToken ct)
        {
            return await sender.Send(new GetIGDBGameDetailsQuery { GameId = id }, ct);
        }

    }
}
