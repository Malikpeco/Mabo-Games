using Market.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Domain.Entities
{
    public class UserAchievementEntity : BaseEntity
    {
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        public int AchievementId { get; set; }
        public AchievementEntity Achievement { get; set; }
        public DateTime AchievedAt { get; set; }
        public UserAchievementEntity()
        {
            AchievedAt = DateTime.UtcNow;
        }
    }
}
