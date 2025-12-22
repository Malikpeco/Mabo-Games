
using Market.Application.Modules.Countries.Commands.Create;
using Market.Application.Modules.Countries.Queries.List;
using Market.Application.Modules.Users.Commands.Register;
using Market.Application.Modules.Users.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController(ISender sender) : ControllerBase
    {
           
        [HttpPost("register")]
        [AllowAnonymous]
        
        public async Task<ActionResult<RegisterUserResultDto>> Register(RegisterUserCommand command, CancellationToken ct)
        {
            var resultDto = await sender.Send(command, ct);

            return Ok(resultDto);
        }

    }
}
