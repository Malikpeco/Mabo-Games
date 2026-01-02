using Market.Application.Modules.Games.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Favourites.Queries.List
{
    public sealed class ListFavouritesQueryHandler(IAppDbContext context, IAppCurrentUser currentUser)
        :IRequestHandler<ListFavouritesQuery, PageResult<StorefrontGameDto>>
    {
        public async Task<PageResult<StorefrontGameDto>> Handle(ListFavouritesQuery request, CancellationToken ct)
        {
            var userId = currentUser.UserId.Value;

            var favourites = context.Favourites
                .AsNoTracking()
                .Where(f => f.UserId == userId).Select(f => f.GameId);

            var projectedQuery = context.Games
                .AsNoTracking()
                .Where(g => favourites.Contains(g.Id))
                .Select(sfg => new StorefrontGameDto
                {
                    Id = sfg.Id,
                    Name = sfg.Name,
                    Price = sfg.Price,
                    ReleaseDate = sfg.ReleaseDate,
                    CoverImageURL = sfg.CoverImageURL,
                    PublisherId = sfg.PublisherId,
                    PublisherName = sfg.Publisher.Name
                });

            return await PageResult<StorefrontGameDto>.FromQueryableAsync(projectedQuery, request.Paging, ct);



        }
    }
}
