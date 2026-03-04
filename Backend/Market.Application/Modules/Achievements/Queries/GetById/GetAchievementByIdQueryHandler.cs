using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Achievements.Queries.GetById
{
    public sealed class GetAchievementByIdQueryHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<GetAchievementByIdQuery, GetAchievementByIdQueryDto>
    {
        public async Task<GetAchievementByIdQueryDto> Handle(GetAchievementByIdQuery request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this.");


            var projectedQuery = await context.Achievements
                .AsNoTracking()
                .Where(a => a.Id == request.Id)
                .Select(a => new GetAchievementByIdQueryDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    ImageURL = a.ImageURL,
                    UserAchievements = a.UserAchievements.Where(ua => ua.UserId == currentUser.UserId.Value)
                    .Select(ua => new UserAchievementDto
                    {
                        UserId = ua.UserId,
                        Username = ua.User.Username,
                        AchievedAt = ua.AchievedAt
                    }).ToList()
                }).FirstOrDefaultAsync(ct);

            if (projectedQuery is null)
                throw new MarketNotFoundException("Achievement not found.");

            return projectedQuery;
        }
    }
}
