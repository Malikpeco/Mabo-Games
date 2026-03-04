using Market.Application.Modules.Genres.Commands.Create;
using Market.Application.Modules.Genres.Queries.List;
using Market.Application.Modules.SecurityQuestions.Commands.Create;
using Market.Application.Modules.SecurityQuestions.Dto;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenresController(ISender sender) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<List<ListGenresQueryDto>> List([FromQuery]ListGenresQuery query, CancellationToken ct)
        {
            return await sender.Send(query, ct);
        }


        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateGenreCommand command, CancellationToken ct)
        { 

            return await sender.Send(command, ct);
        }


    }
}
