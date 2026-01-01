using Market.Application.Modules.Countries.Commands.Create;
using Market.Application.Modules.Countries.Commands.Delete;
using Market.Application.Modules.Countries.Commands.Update;
using Market.Application.Modules.Countries.Queries.GetById;
using Market.Application.Modules.Countries.Queries.List;

namespace Market.API.Controllers;

[ApiController]
[Route("api/countries")]
public class CountriesController(ISender sender) : ControllerBase
{
    // POST /countries      
    [HttpPost]
    public async Task<ActionResult> Create(CreateCountryCommand command, CancellationToken ct)
    {
        await sender.Send(command, ct);

        return Created();
    }

    // GET /countries
    [HttpGet]
    public async Task<PageResult<ListCountriesQueryDto>> List([FromQuery]ListCountriesQuery query, CancellationToken ct)
    {
        return await sender.Send(query, ct);
    }


    //delete
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] int id, CancellationToken ct)
    {
        await sender.Send(new DeleteCountryCommand { Id = id }, ct);
        return NoContent();
    }


    //update
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateCountryCommand command, CancellationToken ct)
    {
        command.Id = id;
        await sender.Send(command, ct);
        return NoContent();
    }


    [HttpGet("{id:int}")]
    public async Task<GetCountryByIdQueryDto> GetById([FromRoute] int id, CancellationToken ct)
    {
        var result = await sender.Send(new GetCountryByIdQuery { Id = id }, ct);
        return result;
    }


}
