using Market.Application.Modules.Games.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Games.Queries.GetGameDetails
{
    public sealed class GetGameDetailsQueryHandler(IAppDbContext context)
        : IRequestHandler<GetGameDetailsQuery, GameDetailsDto>
    {
        public async Task<GameDetailsDto> Handle(GetGameDetailsQuery request, CancellationToken cancellationToken)
        {
            var game = await context.Games
                .AsNoTracking()
                .Where(g => g.Id == request.Id)
                .Select(g => new GameDetailsDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    Price = g.Price,
                    ReleaseDate = g.ReleaseDate,
                    Description = g.Description,
                    CoverImageURL = g.CoverImageURL,
                    Publisher = new PublisherDto
                    {
                        Id = g.Publisher.Id,
                        Name = g.Publisher.Name,
                        CountryId = g.Publisher.CountryId,
                        CountryName = g.Publisher.Country.Name
                    },
                    Screenshots = g.Screenshots.OrderBy(s=>s.Id).Select(s=>new GameScreenshotsDto
                    {
                        ImageURL= s.ImageURL,
                        GameId= s.GameId,
                    }).ToList(),
                    Genres = g.GameGenres.Select(gg => new GameGenreDto
                    {
                        Id = gg.GenreId,
                        Name = gg.Genre.Name,
                    }).ToList(),
                    Reviews = new ReviewSummaryDto
                    {
                        Count = g.UserGames.Where(ug => ug.Review != null).Count(),
                        AverageRating = g.UserGames.Where(ug => ug.Review != null).Select(ug => (float?)ug.Review.Rating).Average() ?? 0f,
                        Items = g.UserGames.Where(ug => ug.Review != null).OrderByDescending(ug => ug.Review!.Date).Take(20)
                        .Select(ug => new ReviewDto
                        {
                            Id = ug.Review!.Id,
                            Rating = ug.Review!.Rating,
                            Content = ug.Review!.Content,
                            Date = ug.Review!.Date,
                            UserId = ug.UserId,
                            Username = ug.User.Username
                        }).ToList()
                    }
                }).FirstOrDefaultAsync();

            if(game is null)
            {
                throw new KeyNotFoundException("Game was not found.");
            }

            return game;

        }
    }
}
