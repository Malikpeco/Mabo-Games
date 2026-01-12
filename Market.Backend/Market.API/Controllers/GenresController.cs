using Market.Application.Modules.Genres.Queries.List;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenresController(ISender sender) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<PageResult<ListGenresQueryDto>> List([FromQuery]ListGenresQuery query, CancellationToken ct)
        {
            return await sender.Send(query, ct);
        }
    }
}
