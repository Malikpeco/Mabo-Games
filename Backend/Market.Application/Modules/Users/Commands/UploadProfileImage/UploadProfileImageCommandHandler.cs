namespace Market.Application.Modules.Users.Commands.UploadProfileImage;

public sealed class UploadProfileImageCommandHandler(
    IAppCurrentUser currentUser,
    IAppDbContext context,
    IBlobStorageService blobStorage)
    : IRequestHandler<UploadProfileImageCommand, Unit>
{
    public async Task<Unit> Handle(UploadProfileImageCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated)
        {
            throw new MarketForbiddenException();
        }

        var user = await context.Users.FirstAsync(x => x.Id == currentUser.UserId.Value, cancellationToken);

        using var fileStream = request.File.OpenReadStream();

        try
        {
            var imageUrl = await blobStorage.UploadImageAsync(fileStream, request.File.FileName, cancellationToken);
            user.ProfileImageURL = imageUrl;

            await context.SaveChangesAsync(cancellationToken);  

            return Unit.Value;
        }
        catch
        {
            throw new MarketConflictException("The image service is temporarily unavailable. Please try again later.");
        }
    }
}