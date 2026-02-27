using Market.Application.Modules.Genres.Queries.List;
using Market.Application.Modules.IGDB.Queries.SearchIGDBGames;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/igdb")]
    public class IGDBController(ISender sender) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<List<SearchIGDBGamesQueryDto>> List([FromQuery] SearchIGDBGamesQuery query, CancellationToken ct)
        {
            return await sender.Send(query, ct);
        }
    }
}
