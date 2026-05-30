
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Stripe;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Supabase.Queries
{
    public sealed class GetGameDownloadUrlQueryHandler(IAppCurrentUser currentUser, ISupaBaseService supaBaseService, IAppDbContext context)
        : IRequestHandler<GetGameDownloadUrlQuery, string>
    {
        public async Task<string> Handle(GetGameDownloadUrlQuery request, CancellationToken ct)
        {
            
            if(!currentUser.IsAuthenticated)
                throw new MarketForbiddenException();

            var game= await context.Games.AsNoTracking().Where(x=>x.Id==request.GameId).FirstOrDefaultAsync(ct);

            if (game == null)
                throw new MarketNotFoundException("Game was not found!");


            var isOwner = await context.UserGames.Where(x => x.GameId == request.GameId && x.UserId == currentUser.UserId).AnyAsync(ct);

            if (!isOwner)
                throw new MarketForbiddenException();

            return await supaBaseService.GetSignedUrlAsync(game.GameFilePath, 60, ct);



        }
    }
}
