using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Dto
{
    public sealed class UserProfileAchievementDto
    {

        public string Name { get; set; }
        public string? Description { get; set; }
        public string ImageURL { get; set; }

        public DateTime UnlockedAt { get; set; }
    }
}
