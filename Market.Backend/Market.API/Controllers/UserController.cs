
using Market.Application.Modules.Countries.Commands.Create;
using Market.Application.Modules.Countries.Queries.List;
using Market.Application.Modules.Users.Commands.PasswordReset;
using Market.Application.Modules.Users.Commands.Register;
using Market.Application.Modules.Users.Commands.RequestPasswordResetBySecurityQuestion;
using Market.Application.Modules.Users.Commands.ResetPassword;
using Market.Application.Modules.Users.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
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

        [HttpPost("password-reset/email")]
        [AllowAnonymous]
        public async Task<ActionResult<Unit>> RequestPasswordResetByEmail(RequestPasswordResetByEmailCommand command, CancellationToken ct)
        {
            await sender.Send(command, ct);

            return Ok(new { message = "A password reset code has been sent. Please check you email." });

        }

        [HttpPost("password-reset/security-question")]
        [AllowAnonymous]
        public async Task<ActionResult<PasswordResetCodeDto>> RequestPasswordResetBySecurityQuestion(RequestPasswordResetBySecurityQuestionCommand command, CancellationToken ct)
        {
            var resultDto = await sender.Send(command, ct);

            return Ok(resultDto);

        }

        [HttpPut("password-reset/email")]
        [AllowAnonymous]
        public async Task<ActionResult<Unit>> PasswordReset(PasswordResetCommand command, CancellationToken ct)
        {
            await sender.Send(command, ct);

            return Ok(new { message = "Password has been reset." });

        }
    }
}
