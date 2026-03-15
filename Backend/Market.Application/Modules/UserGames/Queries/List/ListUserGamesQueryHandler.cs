using Market.Application.Modules.Games.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.UserGames.Queries.List
{
    public sealed class ListUserGamesQueryHandler(IAppCurrentUser currentUser, IAppDbContext context)
        :IRequestHandler<ListUserGamesQuery, PageResult<ListUserGamesQueryDto>>
    {
        public async Task<PageResult<ListUserGamesQueryDto>> Handle(ListUserGamesQuery request, CancellationToken ct)
        {
            var userId = currentUser.UserId.Value;

            var q = context.UserGames.AsNoTracking().Where(ug=>ug.UserId==userId);

            context.Games.ToList();
            

            var projectedQuery = q.Select(ug => new ListUserGamesQueryDto
            {
                Id=ug.Id,
                UserId=ug.UserId,
                GameId=ug.GameId,
                Game= new StorefrontGameDto
                {
                    Id =ug.GameId,
                    Name =ug.Game.Name,
                    Price =ug.Game.Price,
                    ReleaseDate=ug.Game.ReleaseDate,
                    CoverImageURL =ug.Game.CoverImageURL,
                    PublisherId =ug.Game.PublisherId,
                    PublisherName =ug.Game.Publisher.Name,
                    Screenshots = ug.Game.Screenshots.OrderBy(s => s.Id).Select(s => new GameScreenshotsDto
                    {
                        ImageURL = s.ImageURL,
                        GameId = s.GameId,
                    }).ToList(),
                    Genres = ug.Game.GameGenres.Select(gg => new GameGenreDto
                    {
                        Id = gg.GenreId,
                        Name = gg.Genre.Name
                    }).ToList(),
                }
            });

            return await PageResult<ListUserGamesQueryDto>.FromQueryableAsync(projectedQuery, request.Paging, ct);

        }
    }
}
