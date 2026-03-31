using Market.Application.Modules.Genres.Commands.Create;
using Market.Application.Modules.Genres.Commands.Delete;
using Market.Application.Modules.Genres.Commands.Update;
using Market.Application.Modules.Genres.Queries.List;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public sealed class GenresController(ISender sender) : ControllerBase
    {
        [HttpGet]
        public async Task<PageResult<ListGenresQueryDto>> List([FromQuery] ListGenresQuery query, CancellationToken ct)
        {
            return await sender.Send(query, ct);
        }


        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateGenreCommand command, CancellationToken ct)
        { 

            return await sender.Send(command, ct);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute]int id, UpdateGenreCommand command, CancellationToken ct)
        {
            command.Id= id;
            await sender.Send(command, ct);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id, CancellationToken ct)
        {
            return Ok(await sender.Send(new DeleteGenreCommand { Id = id }, ct));
        }


    }
}
