using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Achievements.Queries.GetById
{
    public sealed class GetAchievementByIdQueryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string ImageURL { get; set; }
        public IReadOnlyList<UserAchievementDto> UserAchievements { get; set; } = new List<UserAchievementDto>();
    }

    public sealed class UserAchievementDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public DateTime AchievedAt { get; set; }

    }
}
