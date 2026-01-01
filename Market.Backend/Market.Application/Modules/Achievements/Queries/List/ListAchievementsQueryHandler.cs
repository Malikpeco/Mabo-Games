using Market.Application.Modules.Achievements.Queries.List;
using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Achievements.Queries.List
{
    public sealed class ListAchievementsQueryHandler(IAppDbContext context, IAppCurrentUser currentUser) : IRequestHandler<ListAchievementsQuery, PageResult<ListAchievementsQueryDto>>
    {
        public async Task<PageResult<ListAchievementsQueryDto>> Handle(ListAchievementsQuery request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this!");

            var q = context.Achievements.AsNoTracking();

            if (!string.IsNullOrEmpty(request.Search))
            {
                q = q.Where(a => a.Name.ToLower().Contains((request.Search).ToLower()));
            }


            var projectedQuery = q.OrderBy(a => a.Id)
                .Select(a => new ListAchievementsQueryDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    ImageURL = a.ImageURL,
                });

            return await PageResult<ListAchievementsQueryDto>.FromQueryableAsync(projectedQuery, request.Paging, ct);

        }

    }
}
