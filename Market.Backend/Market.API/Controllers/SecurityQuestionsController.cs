using Market.Application.Modules.Countries.Commands.Create;
using Market.Application.Modules.Countries.Queries.List;
using Market.Application.Modules.SecurityQuestions.Commands.Create;
using Market.Application.Modules.SecurityQuestions.Commands.Delete;
using Market.Application.Modules.SecurityQuestions.Commands.Update;
using Market.Application.Modules.SecurityQuestions.Dto;
using Market.Application.Modules.SecurityQuestions.Queries.GetById;
using Market.Application.Modules.SecurityQuestions.Queries.List;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers
{

    [ApiController]
    [Route("api/security-questions")]
    public class SecurityQuestionsController(ISender sender) : ControllerBase
    {

        [HttpPost]
        public async Task<ActionResult<CreateSecurityQuestionResultDto>> Create(CreateSecurityQuestionCommand command, CancellationToken ct)
        {
            var resultDto = await sender.Send(command, ct);

            return CreatedAtAction(nameof(GetById), new { id = resultDto.Id},resultDto);
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<UpdateSecurityQuestionResultDto>> Update(int id, UpdateSecurityQuestionCommand command, CancellationToken ct)
        {
            command.Id = id;
            var result =await sender.Send(command, ct);

            return Ok(result);

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<DeleteSecurityQuestionResultDto>> Delete(int id, CancellationToken ct)
        {
            var result = await sender.Send(new DeleteSecurityQuestionCommand { Id = id }, ct);

            return Ok(result);
        }



        [HttpGet]

        public async Task<ActionResult<List<ListSecurityQuestionsQueryDto>>> GetAll(CancellationToken ct)
        {
            var result = await sender.Send(new ListSecurityQuestionsQuery(), ct);
            return Ok(result);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetSecurityQuestionsByIdQueryDto>> GetById(int id, CancellationToken ct)
        {
            var result = await sender.Send(new GetSecurityQuestionsByIdQuery { Id = id }, ct);
            return Ok(result); 
        }

    }
}
