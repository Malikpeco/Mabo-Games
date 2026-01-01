using Market.Application.Modules.Achievements.Commands.Create;
using Market.Application.Modules.Achievements.Commands.Delete;
using Market.Application.Modules.Achievements.Commands.Update;
using Market.Application.Modules.Achievements.Queries.GetById;
using Market.Application.Modules.Achievements.Queries.List;
using Market.Infrastructure.Database.Configurations;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/achievements")]
    public class AchievementsController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Create(CreateAchievementCommand command, CancellationToken ct)
        {
            await sender.Send(command, ct);
            return Ok();
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id, CancellationToken ct)
        {
            await sender.Send(new DeleteAchievementCommand { Id = id }, ct);
            return NoContent();
        }



        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(UpdateAchievementCommand command, int id, CancellationToken ct)
        {
            command.Id = id;
            await sender.Send(command, ct);
            return NoContent();
        }


        [HttpGet]
        public async Task<PageResult<ListAchievementsQueryDto>> List([FromQuery] ListAchievementsQuery query, CancellationToken ct)
        {
            return await sender.Send(query, ct);
            
        }



        [HttpGet("{id:int}")]
        public async Task<GetAchievementByIdQueryDto> GetById([FromRoute] int id, CancellationToken ct)
        {
            return await sender.Send(new GetAchievementByIdQuery { Id = id}, ct);
        }


    }
}
