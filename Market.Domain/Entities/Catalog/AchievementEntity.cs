using Market.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Domain.Entities
{
    public class AchievementEntity : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string ImageURL { get; set; }
        public IReadOnlyCollection<UserAchievementEntity> UserAchievements { get; private set; } = new List<UserAchievementEntity>();
    }
}
