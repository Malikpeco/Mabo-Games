using Market.Application.Common.Achievements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Abstractions
{
    public interface IAchievementSystem
    {
        Task TryAwardAsync(int userId, string achievementName, CancellationToken ct);

        Task CheckEligibilityAsync(int userId, AchievementTriggerType trigger, CancellationToken ct);
    }
}
