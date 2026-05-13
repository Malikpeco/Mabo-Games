using Microsoft.AspNetCore.Http;

namespace Market.Application.Modules.Achievements.Commands.UploadImage;

public sealed class UploadAchievementImageCommand : IRequest<string>
{
    public IFormFile File { get; set; }
}
