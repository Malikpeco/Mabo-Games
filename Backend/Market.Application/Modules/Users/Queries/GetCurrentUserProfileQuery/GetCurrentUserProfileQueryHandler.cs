using Market.Application.Modules.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Queries.GetCurrentUserProfileQuery
{
    public sealed class GetCurrentUserProfileQueryHandler(IAppCurrentUser currentUser, IAppDbContext context) : IRequestHandler<GetCurrentUserProfileQuery, GetUserProfileQueryDto>
    {
        public async Task<GetUserProfileQueryDto> Handle(GetCurrentUserProfileQuery request, CancellationToken cancellationToken)
        {
            if (!currentUser.IsAuthenticated || !currentUser.UserId.HasValue)
            {
                throw new MarketForbiddenException();
            }

            var q = context.Users
                .AsNoTracking()
                .Where(x => x.Id == currentUser.UserId.Value);

            var projectedQ =
                q.Select(x => new GetUserProfileQueryDto
                {
                    Username = x.Username,
                    CountryId = x.CountryId,
                    ProfileImageURL = x.ProfileImageURL,
                    Bio = x.ProfileBio,
                    City = x.City != null ? x.City.Name : null,
                    Country = x.Country != null ? x.Country.Name : null,

                    OwnedGamesCount = x.UserGames.Count(),

                    Achievements = x.UserAchievements
                   .OrderByDescending(x => x.AchievedAt)
                   .Select(y => new UserProfileAchievementDto
                   {
                       Name = y.Achievement.Name,
                       Description = y.Achievement.Description,
                       UnlockedAt = y.AchievedAt,
                       ImageURL = y.Achievement.ImageURL

                   })
                   .ToList(),


                    RecentlyBoughtGames = x.UserGames
                    .OrderByDescending(x => x.PurchaseDate)
                    .Take(5)
                    .Select(y => new UserRecentlyBoughtGameDto
                    {
                        CoverImageURL = y.Game.CoverImageURL,
                        Name = y.Game.Name

                    })
                    .ToList(),
                    IsOwnProfile = true,
                });

            return await projectedQ.FirstOrDefaultAsync(cancellationToken) ?? throw new Exception("User profile not found.");
        }
    }
}
