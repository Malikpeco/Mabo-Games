using Market.Application.Modules.Games.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.UserGames.Queries.List
{
    public sealed class ListUserGamesQueryHandler(IAppCurrentUser currentUser, IAppDbContext context)
        :IRequestHandler<ListUserGamesQuery, PageResult<StorefrontGameDto>>
    {
        public async Task<PageResult<StorefrontGameDto>> Handle(ListUserGamesQuery request, CancellationToken ct)
        {
            var userId = currentUser.UserId.Value;

            var q = context.Games.AsNoTracking().Where(g=>g.UserGames.Any(ug=>ug.UserId==userId));

            var projectedQuery = q.Select(g => new StorefrontGameDto
            {
                Id = g.Id,
                Name = g.Name,
                Price = g.Price,
                ReleaseDate = g.ReleaseDate,
                CoverImageURL = g.CoverImageURL,
                PublisherId = g.PublisherId,
                PublisherName = g.Publisher.Name
            });

            return await PageResult<StorefrontGameDto>.FromQueryableAsync(projectedQuery, request.Paging, ct);

        }
    }
}
