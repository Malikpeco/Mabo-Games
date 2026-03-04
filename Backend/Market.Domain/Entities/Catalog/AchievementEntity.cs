using Market.Domain.Common;
using Market.Domain.Common.Attributes;

namespace Market.Domain.Entities
{
    public class AchievementEntity : BaseEntity
    {
       
        public string Name { get; set; }

        [PreserveString]
        public string? Description { get; set; }

        [PreserveString]
        public string ImageURL { get; set; }
        public IReadOnlyCollection<UserAchievementEntity> UserAchievements { get; private set; } = new List<UserAchievementEntity>();
    }
}
