using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Reviews.Queries.List
{
    public sealed class ListReviewsQueryHandler(IAppDbContext context)
        :IRequestHandler<ListReviewsQuery, PageResult<ListReviewsQueryDto>>
    {
        public async Task<PageResult<ListReviewsQueryDto>> Handle(ListReviewsQuery request, CancellationToken ct)
        {
            var q = context.Reviews.AsNoTracking();

            if (request.GameId.HasValue)
            {
                q = q.Where(r => r.UserGame.GameId == request.GameId.Value);
            }

            var projectedQuery = q.Select(r => new ListReviewsQueryDto
            {
                Content = r.Content,
                Rating = r.Rating,
                Date = r.Date,
                UserGame = new ReviewUserGameDto
                {
                    Id = r.UserGame.Id,
                    User = new ReviewUserDto
                    {
                        Id = r.UserGame.User.Id,
                        Username = r.UserGame.User.Username,
                    },
                    Game = new ReviewGameDto
                    {
                        Id = r.UserGame.Game.Id,
                        Name = r.UserGame.Game.Name,
                    },
                    PurchaseDate = r.UserGame.PurchaseDate,
                }
            }).OrderByDescending(r=>r.Date);


            return await PageResult<ListReviewsQueryDto>.FromQueryableAsync(projectedQuery,request.Paging, ct);


        }
    }
}
