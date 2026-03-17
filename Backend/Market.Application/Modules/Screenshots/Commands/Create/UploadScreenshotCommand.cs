using Microsoft.AspNetCore.Http;

namespace Market.Application.Modules.Screenshots.Commands.Create
{
    public sealed class UploadScreenshotCommand : IRequest<string>
    {
        public IFormFile File { get; set; }
    }
}
