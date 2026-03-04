using Market.Application.Modules.SecurityQuestions.Queries.List;
using Market.Application.Modules.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Queries.GetUserProfileQuery
{
    public sealed class GetUserProfileQueryHandler(IAppCurrentUser currentUser, IAppDbContext context) : IRequestHandler<GetUserProfileQuery, GetUserProfileQueryDto>
    {
        public async Task<GetUserProfileQueryDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {

            var q = context.Users
                .AsNoTracking()
                .Where(x=>x.Username==request.Username);


            var projectedQ =
                q.Select(x => new GetUserProfileQueryDto
                {
                    Username = x.Username,
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


                    RecentlyBoughtGames= x.UserGames
                    .OrderByDescending (x=>x.PurchaseDate)
                    .Take(5)
                    .Select(y=>new UserRecentlyBoughtGameDto
                    {
                        GameId = y.GameId,
                        Name=y.Game.Name,
                        CoverImageURL=y.Game.CoverImageURL,
                        
                    })
                    .ToList(),


                    IsOwnProfile = x.Id == currentUser.UserId

                });


            var userProfile = await projectedQ.FirstOrDefaultAsync(cancellationToken);

            if (userProfile == null)
                throw new MarketNotFoundException($"User {request.Username} was not found");

            return userProfile;

        }
    }
}
