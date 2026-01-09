using Market.Application.Modules.Games.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Games.Queries.GetStorefrontGames
{
    public sealed class GetStorefrontGamesQueryHandler(IAppDbContext context)
        : IRequestHandler<GetStorefrontGamesQuery, PageResult<StorefrontGameDto>>
    {
        public async Task<PageResult<StorefrontGameDto>> Handle(GetStorefrontGamesQuery request, CancellationToken cancellationToken)
        {
            var q = context.Games.AsNoTracking();

            var searchTerm = request.Search?.Trim().ToLower() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                q = q.Where(x => x.Name.ToLower().Contains(searchTerm));
            }


            if (request.GenreIds is not null  && request.GenreIds.Count>0)
            {
                q=q.Where(x=>x.GameGenres.Any(gg=>request.GenreIds.Contains(gg.GenreId)));
            }


            q = request.Sort switch
            {
                "oldest" => q.OrderBy(x => x.ReleaseDate).ThenBy(x => x.Id),
                "priceAsc" => q.OrderBy(x => x.Price).ThenBy(x => x.Id),
                "priceDesc" => q.OrderByDescending(x => x.Price).ThenBy(x => x.Id),
                "nameAsc" => q.OrderBy(x => x.Name).ThenBy(x => x.Id),
                "nameDesc" => q.OrderByDescending(x => x.Name).ThenBy(x => x.Id),
                _ => q.OrderByDescending(x => x.ReleaseDate).ThenBy(x => x.Id) //default:order by newest release date to oldest
            };


            var projectedQuery = q
                .Select(x => new StorefrontGameDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    ReleaseDate = x.ReleaseDate,
                    CoverImageURL = x.CoverImageURL,
                    PublisherId = x.PublisherId,
                    PublisherName = x.Publisher!.Name,
                    Screenshots = x.Screenshots.OrderBy(s=>s.Id).Select(s=>new GameScreenshotsDto
                    {
                        ImageURL= s.ImageURL,
                        GameId=s.GameId,
                    }).ToList(),
                });

            return await PageResult<StorefrontGameDto>.FromQueryableAsync(projectedQuery, request.Paging, cancellationToken);
        }
    }
}
