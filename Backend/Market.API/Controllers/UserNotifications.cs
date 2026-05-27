using Market.Application.Modules.UserNotifications.Commands.Broadcast;
using Market.Application.Modules.UserSecurityQuestions.Commands.Create;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/user-notificatios")]
    public class UserNotifications(ISender sender) : ControllerBase
    {
        [HttpPost("NotifyAll")]
        public async Task<ActionResult<Unit>> Create(NotifyAllUsersCommand command, CancellationToken ct)
        {
            return await sender.Send(command, ct);
        }



    }


}


