using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Supabase.Commands
{
    public sealed class UploadGameFileCommandHandler(IAppCurrentUser currentUser, ISupaBaseService supaBaseService) : IRequestHandler<UploadGameFileCommand, string>
    {
        public async Task<string> Handle(UploadGameFileCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.IsAdmin)
                throw new MarketForbiddenException();


            using var fileStream = request.File.OpenReadStream();


            try
            {
                return await supaBaseService.UploadFileAsync(fileStream, request.File.FileName, cancellationToken);
            }
            catch (Exception ex)
            {

                Console.WriteLine( $"UploadFileAsync failed: {ex} " ); throw new MarketConflictException( $"Supabase storage upload failed. Details: {ex.Message} " );
            }
        }
    }
}





