using Market.Application.Modules.Countries.Commands.Create;
using Market.Application.Modules.SecurityQuestions.Commands.Create;
using Market.Application.Modules.SecurityQuestions.Commands.Update;
using Market.Application.Modules.SecurityQuestions.Dto;
using Market.Application.Modules.SecurityQuestions.Queries.GetById;
using Market.Application.Modules.UserSecurityQuestions.Commands.Create;
using Market.Application.Modules.UserSecurityQuestions.Commands.Remove;
using Market.Application.Modules.UserSecurityQuestions.Commands.Update;
using Market.Application.Modules.UserSecurityQuestions.Queries.GetById;
using Market.Application.Modules.UserSecurityQuestions.Queries.List;
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

        [HttpPut("{id:int}")]
        public async Task<ActionResult<int>> Update(int id, UpdateUserSecurityQuestionsCommand command, CancellationToken ct)
        {
            command.Id = id;
            var result = await sender.Send(command, ct);

            return Ok(result);

        }

        [HttpDelete("{id:int}")]

        public async Task<ActionResult<int>> Remove(int id, RemoveUserSecurityQuestionCommand command,CancellationToken ct)
        {
            command.Id = id;
            var result = await sender.Send(command, ct);

            return Ok(result);
        }


        [HttpGet]

        public async Task<ActionResult<List<ListUserSecurityQuestionsQueryDto>>> List([FromQuery]ListUserSecurityQuestionsQuery query, CancellationToken ct)
        {
            var result = await sender.Send(query, ct);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetUserSecurityQuestionsByIdQueryDto>> GetById(int id, CancellationToken ct)
        {
            var result = await sender.Send(new GetUserSecurityQuestionsByIdQuery { Id = id }, ct);
            return Ok(result); 
        }




    }
}
