using Market.Application.Modules.Countries.Commands.Create;
using Market.Application.Modules.Countries.Queries.List;
using Market.Application.Modules.RegisterUser.Commands.Register;
using Market.Application.Modules.RegisterUser.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterUserController(ISender sender) : ControllerBase
    {
        // POST /countries      
        [HttpPost]
        public async Task<ActionResult<RegisterUserResultDto>> Create(RegisterUserCommand command, CancellationToken ct)
        {
            var resultDto = await sender.Send(command, ct);

            return Ok(resultDto);
        }

    }
}
