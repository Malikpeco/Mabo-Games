using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Dto
{
    public sealed class GetUserProfileQueryDto
    {

        public string Username { get; set; }

        public string? Bio { get; set; }

        public string? ProfileImageURL{ get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public int OwnedGamesCount { get; set; }

        public bool IsOwnProfile { get; set; }


        public IReadOnlyList<UserProfileAchievementDto> Achievements { get; set; }

        public IReadOnlyList<UserRecentlyBoughtGameDto> RecentlyBoughtGames { get; set; }



    }
}
