using Market.Application.Modules.Cities.Commands.Create;
using Market.Application.Modules.Cities.Commands.Delete;
using Market.Application.Modules.Cities.Commands.Update;
using Market.Application.Modules.Cities.Queries.GetById;
using Market.Application.Modules.Cities.Queries.List;
using Market.Application.Modules.Cities.Queries.ListCitiesAutocomplete;
using Market.Application.Modules.Games.Dto;

namespace Market.API.Controllers
{


    [ApiController]
    [Route("api/cities")]


    public sealed class CitiesController(ISender sender) : ControllerBase
    {

        [HttpGet("{id:int}")]
        public async Task<GetCityByIdQueryDto> GetById([FromRoute] int id, CancellationToken ct)
        {
            return await sender.Send(new GetCityByIdQuery { Id = id }, ct);
        }

        [HttpGet]
        public async Task<PageResult<ListCitiesQueryDto>> List([FromQuery] ListCitiesQuery query, CancellationToken ct)
        {
            return await sender.Send(query, ct);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateCityCommand command, CancellationToken ct)
        {
            var cityid = await sender.Send(command, ct);
            return CreatedAtAction("GetById", new { id = cityid }, cityid);
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id, CancellationToken ct)
        {
            await sender.Send(new DeleteCityCommand { Id = id }, ct);
            return NoContent();
        }



        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody]UpdateCityCommand command, CancellationToken ct)
        {
            command.Id = id;
            return Ok(await sender.Send(command, ct));

        }

        [HttpGet("autocomplete")]
        public async Task<List<ListCitiesAutocompleteQueryDto>> Autocomplete([FromQuery] string term, CancellationToken ct)
        {
            return await sender.Send(new ListCitiesAutocompleteQuery { Term = term }, ct);
        }

    }
}
