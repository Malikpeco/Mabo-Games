using Market.Application.Common.Achievements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Abstractions
{
    public interface IAchievementEvaluator
    {
        AchievementTriggerType TriggerType { get; }
        Task EvaluateAsync(int userId, IAppDbContext context, IAchievementSystem achievementSystem, CancellationToken ct);
    }
}
