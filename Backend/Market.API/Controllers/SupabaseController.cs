using Market.Application.Modules.Games.Dto;
using Market.Application.Modules.Games.Queries.GetGameDetails;
using Market.Application.Modules.Supabase.Commands;
using Market.Application.Modules.Supabase.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers
{
    [Route("api/supabase")]
    [ApiController]
    public class SupabaseController(ISender sender) : ControllerBase
    {
        [Consumes("multipart/form-data")]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file, CancellationToken ct)
        {
            var command = new UploadGameFileCommand() { File = file };

            var filePath = await sender.Send(command, ct);

            return Ok(new { filePath = filePath });
        }



        
        [HttpGet("{id:int}")]
        public async Task<string> GetGameDownloadUrlQuery(int id, CancellationToken ct)
        {
            return await sender.Send(new GetGameDownloadUrlQuery { GameId = id }, ct);
        }




    }
}
