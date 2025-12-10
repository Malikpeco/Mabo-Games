using Market.Application.Modules.Countries.Commands.Create;
using Market.Application.Modules.Countries.Queries.List;
using Market.Application.Modules.SecurityQuestions.Commands.Create;
using Market.Application.Modules.SecurityQuestions.Commands.Delete;
using Market.Application.Modules.SecurityQuestions.Dto;
using Market.Application.Modules.SecurityQuestions.Queries.GetById;
using Market.Application.Modules.SecurityQuestions.Queries.List;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class SecurityQuestionsController(ISender sender) : ControllerBase
    {

        [HttpPost]
        public async Task<ActionResult<CreateSecurityQuestionResultDto>> Create(CreateSecurityQuestionsCommand command, CancellationToken ct)
        {
            var resultDto = await sender.Send(command, ct);

            return Ok(resultDto);
        }

        [HttpGet]

        public async Task<List<ListSecurityQuestionsQueryDto>> GetAll(CancellationToken ct)
        {
            var result = await sender.Send(new ListSecurityQuestionsQuery(), ct);
            return result;
        }


        [HttpGet("{id:int}")]
        public async Task<GetSecurityQuestionsByIdQueryDto> GetById(int id, CancellationToken ct)
        {
            var result = await sender.Send(new GetSecurityQuestionsByIdQuery { Id = id }, ct);
            return result; // if NotFoundException -> 404 via middleware
        }



        [HttpDelete]
        public async Task<ActionResult<DeleteSecurityQuestionResultDto>> Delete(int id, CancellationToken ct)
        {
            var result = await sender.Send(new DeleteSecurityQuestionsCommand { Id = id }, ct);

            return Ok(result);
        }



    }
}
