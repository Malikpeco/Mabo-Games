using Market.Application.Modules.Countries.Commands.Create;
using Market.Application.Modules.SecurityQuestions.Commands.Create;
using Market.Application.Modules.SecurityQuestions.Dto;
using Market.Application.Modules.SecurityQuestions.Queries.GetById;
using Market.Application.Modules.UserSecurityQuestions.Commands.Create;
using Market.Application.Modules.UserSecurityQuestions.Queries.GetById;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers
{

    [ApiController]
    [Route("api/user-security-questions")]
    public class UserSecurityQuestionsController(ISender sender) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<int>> Create(RegisterUserSecurityQuestionsCommand command, CancellationToken ct)
        {
            int id = await sender.Send(command, ct);

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetUserSecurityQuestionsByIdQueryDto>> GetById(int id, CancellationToken ct)
        {
            var result = await sender.Send(new GetUserSecurityQuestionsByIdQuery { Id = id }, ct);
            return Ok(result); 
        }


    }
}
