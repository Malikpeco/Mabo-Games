using Market.Application.Abstractions;
using Market.Application.Common.Achievements;
using Market.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Infrastructure.Common.AchievementsSystem
{
    public sealed class AchievementsSystem : IAchievementSystem
    {
        private readonly IAppDbContext _context;
        private readonly ILookup<AchievementTriggerType, IAchievementEvaluator> _evaluators;

        public AchievementsSystem(IAppDbContext context, IEnumerable<IAchievementEvaluator> evaluators)
        {
            _context = context;
            _evaluators = evaluators.ToLookup(e => e.TriggerType);
        }

        public async Task CheckEligibilityAsync(int userId, AchievementTriggerType trigger, CancellationToken ct)
        {
            foreach (var evaluator in _evaluators[trigger])
            {
                await evaluator.EvaluateAsync(userId, _context, this, ct);
            }
        }

        public async Task TryAwardAsync(int userId, string achievementName, CancellationToken ct)
        {
            var achievement = await _context.Achievements
                .FirstOrDefaultAsync(a => a.Name == achievementName, ct);

            if (achievement is null) return;

            bool alreadyHas = await _context.UserAchievements
                .AnyAsync(ua => ua.UserId == userId && ua.AchievementId == achievement.Id, ct);

            if (alreadyHas) return;

            _context.UserAchievements.Add(new UserAchievementEntity
            {
                UserId = userId,
                AchievementId = achievement.Id,
            });

            await _context.SaveChangesAsync(ct);
        }
    }
}