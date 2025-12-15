using Market.Application.Modules.Countries.Commands.Create;
using Market.Application.Modules.Countries.Queries.List;

namespace Market.API.Controllers;

[ApiController]
[Route("api/countries")]
public class CountriesController(ISender sender) : ControllerBase
{
    // POST /countries      
    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateCountryCommand command, CancellationToken ct)
    {
        int id = await sender.Send(command, ct);

        return CreatedAtAction(nameof(GetAll), new { id }, new { id });
    }

    // GET /countries
    [HttpGet]
    public async Task<PageResult<ListCountriesQueryDto>> GetAll(CancellationToken ct)
    {
        var result = await sender.Send(new ListCountriesQuery(), ct);
        return result;  
    }
}
