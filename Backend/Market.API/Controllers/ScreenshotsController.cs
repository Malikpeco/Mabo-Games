using Market.Application.Modules.IGDB.Queries.GetIGDBGameDetails;
using Market.Application.Modules.IGDB.Queries.SearchIGDBGames;
using Market.Application.Modules.Screenshots.Commands.Create;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers
{
    [Route("api/screenshots")]
    [ApiController]
    public class ScreenshotsController(ISender sender) : ControllerBase
    {
        [AllowAnonymous]
        [Consumes("multipart/form-data")]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile imageFile, CancellationToken ct)
        {
            var command = new UploadScreenshotCommand() { File = imageFile };

            var imageUrl = await sender.Send(command, ct);

            return Ok(new { url = imageUrl });
        }


    }
}
