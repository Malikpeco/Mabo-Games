using Microsoft.AspNetCore.Http;

namespace Market.Application.Modules.Users.Commands.UploadProfileImage;

public sealed class UploadProfileImageCommand : IRequest<Unit>
{
    public IFormFile File { get; set; }
}