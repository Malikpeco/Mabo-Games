using Market.Application.Modules.UserNotifications.Commands.Broadcast;
using Market.Application.Modules.UserNotifications.Commands.ClearRead;
using Market.Application.Modules.UserNotifications.Queries.GetById;
using Market.Application.Modules.UserNotifications.Queries.List;
using Market.Application.Modules.UserSecurityQuestions.Commands.Create;
using Market.Application.Modules.UserSecurityQuestions.Queries.GetById;
using Market.Application.Modules.UserSecurityQuestions.Queries.List;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/user-notifications")]
    public class UserNotificationsController(ISender sender) : ControllerBase
    {
        [HttpPost("NotifyAll")]
        public async Task<ActionResult<Unit>> Create(NotifyAllUsersCommand command, CancellationToken ct)
        {
            return await sender.Send(command, ct);
        }

        [HttpDelete("ClearRead")]
        public async Task<ActionResult<Unit>> ClearRead(CancellationToken ct)
        {
            return await sender.Send(new ClearReadUserNotificationsCommand(), ct);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetUserNotificationsByIdQueryDto>> GetById(int id, CancellationToken ct)
        {
            var result = await sender.Send(new GetUserNotificationsByIdQuery { Id = id }, ct);
            return Ok(result);
        }


        [HttpGet("list")]
        public async Task<ActionResult<List<ListUserNotificationsQueryDto>>> List(
        [FromQuery] ListUserNotificationsQuery query, CancellationToken ct)
        {
            var result = await sender.Send(query, ct);
            return Ok(result);
        }




    }


}


