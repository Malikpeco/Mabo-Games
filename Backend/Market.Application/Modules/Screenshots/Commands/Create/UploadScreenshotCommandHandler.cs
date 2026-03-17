namespace Market.Application.Modules.Screenshots.Commands.Create
{
    public sealed class UploadScreenshotCommandHandler(IAppCurrentUser currentUser,IAppDbContext context,IBlobStorageService blobStorage) 
        : IRequestHandler<UploadScreenshotCommand, string>
    {
        public async Task<string> Handle(UploadScreenshotCommand request, CancellationToken cancellationToken)
        {
            //NOTE TO SELF UNTAG AND MAKE RESTRICTED IN CONTROLLER

            //if(!currentUser.IsAdmin)
                //throw new MarketForbiddenException();


            using var fileStream=request.File.OpenReadStream();


            try
            {
                return await blobStorage.UploadImageAsync(fileStream, request.File.FileName, cancellationToken);
            }
            catch (Exception ex)
            {
                
                throw new MarketConflictException("The image service is temporarily unavailable. Please try again later.");
            }
        }
    }

}
