using Market.Application.Abstractions;
using Market.Application.Common.Achievements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.Common.AchievementsSystem.Evaluators
{
    public sealed class ReviewAchievementEvaluator : IAchievementEvaluator
    {
        public AchievementTriggerType TriggerType => AchievementTriggerType.ReviewSubmitted;

        public async Task EvaluateAsync(int userId, IAppDbContext context, IAchievementSystem achievementSystem, CancellationToken ct)
        {
            int reviewCount = await context.Reviews.CountAsync(r => r.UserGame.UserId == userId, ct);

            if (reviewCount >= 1)
            {
                await achievementSystem.TryAwardAsync(userId, "I know my taste", ct);
            }
            else if (reviewCount >= 10)
            {
                await achievementSystem.TryAwardAsync(userId, "Critic", ct);
            }
        }
    }
}
