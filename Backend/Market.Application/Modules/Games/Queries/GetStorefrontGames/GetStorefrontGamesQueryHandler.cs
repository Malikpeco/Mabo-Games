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
        private const int MinReviewsForRating = 3;

        public async Task<PageResult<StorefrontGameDto>> Handle(GetStorefrontGamesQuery request, CancellationToken cancellationToken)
        {
            var q = context.Games.AsNoTracking();

            var searchTerm = request.Search?.Trim().ToLower() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                q = q.Where(x =>
                    x.Name.ToLower().Contains(searchTerm) ||
                    x.Publisher.Name.ToLower().Contains(searchTerm));
            }


            if (request.GenreIds is not null  && request.GenreIds.Count>0)
            {
                q=q.Where(x=>request.GenreIds.All(genreId=>x.GameGenres.Any(gg=>gg.GenreId==genreId)));
            }

            if (request.MinPrice.HasValue)
            {
                q = q.Where(x => x.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                q = q.Where(x => x.Price <= request.MaxPrice.Value);
            }


            switch (request.Sort)
            {
                case "oldest":
                    q = q.OrderBy(x => x.ReleaseDate).ThenBy(x => x.Id);
                    break;
                case "priceAsc":
                    q = q.OrderBy(x => x.Price).ThenBy(x => x.Id);
                    break;
                case "priceDesc":
                    q = q.OrderByDescending(x => x.Price).ThenBy(x => x.Id);
                    break;
                case "nameAsc":
                    q = q.OrderBy(x => x.Name).ThenBy(x => x.Id);
                    break;
                case "nameDesc":
                    q = q.OrderByDescending(x => x.Name).ThenBy(x => x.Id);
                    break;
                case "recentlyAdded":
                    q = q.OrderByDescending(x => x.CreatedAtUtc).ThenBy(x => x.Id);
                    break;
                case "topRatedAllTime": //editor's picks: highest average rating of all time
                    q = q.Where(x => x.UserGames.Count(ug => ug.Review != null) >= MinReviewsForRating)
                         .OrderByDescending(x => x.UserGames.Where(ug => ug.Review != null)
                             .Select(ug => (float?)ug.Review!.Rating).Average())
                         .ThenBy(x => x.Id);
                    break;
                case "topRatedWeek": //highest average rating among games reviewed in the last 7 days
                    var weekAgo = DateTime.UtcNow.AddDays(-7);
                    q = q.Where(x => x.UserGames.Count(ug => ug.Review != null) >= MinReviewsForRating
                                   && x.UserGames.Any(ug => ug.Review != null && ug.Review!.Date >= weekAgo))
                         .OrderByDescending(x => x.UserGames.Where(ug => ug.Review != null)
                             .Select(ug => (float?)ug.Review!.Rating).Average())
                         .ThenBy(x => x.Id);
                    break;
                default:
                    q = q.OrderByDescending(x => x.ReleaseDate).ThenBy(x => x.Id); //default:order by newest release date to oldest
                    break;
            }


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
                    Genres = x.GameGenres.Select(gg=>new GameGenreDto
                    {
                        Id=gg.GenreId,
                        Name=gg.Genre.Name
                    }).ToList(),
                    AverageRating = x.UserGames.Where(ug => ug.Review != null)
                        .Select(ug => (float?)ug.Review!.Rating).Average() ?? 0f,
                    ReviewCount = x.UserGames.Count(ug => ug.Review != null),
                });

            return await PageResult<StorefrontGameDto>.FromQueryableAsync(projectedQuery, request.Paging, cancellationToken);
        }
    }
}
