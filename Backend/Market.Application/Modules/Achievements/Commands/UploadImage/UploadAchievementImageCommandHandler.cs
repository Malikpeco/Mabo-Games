namespace Market.Application.Modules.Achievements.Commands.UploadImage;

public sealed class UploadAchievementImageCommandHandler(
    IAppCurrentUser currentUser,
    IBlobStorageService blobStorage)
    : IRequestHandler<UploadAchievementImageCommand, string>
{
    public async Task<string> Handle(UploadAchievementImageCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAdmin)
        {
            throw new MarketForbiddenException();
        }

        using var fileStream = request.File.OpenReadStream();

        try
        {
            return await blobStorage.UploadImageAsync(fileStream, request.File.FileName, cancellationToken);
        }
        catch
        {
            throw new MarketConflictException("The image service is temporarily unavailable. Please try again later.");
        }
    }
}
