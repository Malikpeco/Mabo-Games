using Market.Application.Modules.Users.Commands.ChangeBio;
using Market.Application.Modules.Users.Commands.ChangePassword;
using Market.Application.Modules.Users.Commands.ChangeUsername;
using Market.Application.Modules.Users.Commands.PasswordReset;
using Market.Application.Modules.Users.Commands.Register;
using Market.Application.Modules.Users.Commands.RequestPasswordResetBySecurityQuestion;
using Market.Application.Modules.Users.Commands.ResetPassword;
using Market.Application.Modules.Users.Dto;
using Market.Application.Modules.Users.Queries.CheckRecoveryOptions;
using Market.Application.Modules.Users.Queries.GetUserProfileQuery;
using Market.Application.Modules.Users.Queries.VerifyResetCode;

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

        [HttpPut("password-reset/change")]
        [AllowAnonymous]
        public async Task<ActionResult<Unit>> PasswordReset(PasswordResetCommand command, CancellationToken ct)
        {
            await sender.Send(command, ct);

            return Ok(new { message = "Password has been reset." });

        }

        [HttpGet("{username}")]
        [AllowAnonymous]
        public async Task<ActionResult<GetUserProfileQueryDto>> GetUserProfile(string username, CancellationToken ct)
        {
            var resultDto =
                await sender.Send(new GetUserProfileQuery(username), ct);

            return Ok(resultDto);
        }

        [HttpGet("check-recovery-options")]
        [AllowAnonymous]
        public async Task<ActionResult<CheckRecoveryOptionsDto>> CheckRecoveryOptions(string recoveryEmail, CancellationToken ct)
        {
            var resultDto =
                await sender.Send(new CheckRecoveryOptionsQuery(recoveryEmail), ct);

            return Ok(resultDto);
        }

        [HttpGet("verify-reset-code")]
        [AllowAnonymous]
        public async Task<ActionResult<CheckRecoveryOptionsDto>> VerifyResetCode(string resetCode, string userEmail, CancellationToken ct)
        {
            var resultDto =
                await sender.Send(new VerifyResetCodeQuery(resetCode,userEmail), ct);

            return Ok(resultDto);
        }


        [HttpPut("username-change")]
        public async Task<ActionResult<Unit>> ChangeUsername(ChangeUsernameCommand command, CancellationToken ct)
        {
            await sender.Send(command, ct);

            return Ok(new { message = "Username has been changed." });

        }


        [HttpPut("password-change")]
        public async Task<ActionResult<Unit>> ChangePassword(ChangePasswordCommand command, CancellationToken ct)
        {
            await sender.Send(command, ct);

            return Ok(new { message = "Password has been changed." });

        }

        [HttpPut("profilebio-change")]
        public async Task<ActionResult<Unit>> ChangeProfileBio(ChangeProfileBioCommand command, CancellationToken ct)
        {
            await sender.Send(command, ct);

            return Ok(new { message = "Profile bio has been changed." });

        }


    }
}
