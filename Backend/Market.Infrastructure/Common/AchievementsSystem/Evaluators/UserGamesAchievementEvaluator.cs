using Market.Application.Abstractions;
using Market.Application.Common.Achievements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.Common.AchievementsSystem.Evaluators
{
    public sealed class UserGamesAchievementEvaluator : IAchievementEvaluator
    {
        public AchievementTriggerType TriggerType => AchievementTriggerType.GamePurchased;

        public async Task EvaluateAsync(int userId, IAppDbContext context, IAchievementSystem achievementSystem, CancellationToken ct)
        {
           
            var userGames = await context.UserGames
                .Where(ug => ug.UserId == userId)
                .Include(ug => ug.Game)
                .ToListAsync(ct);

            int totalGamesOwned = userGames.Count;

           //(First Blood, Collector, Hoarder)
            if (totalGamesOwned >= 1) await achievementSystem.TryAwardAsync(userId, "First Blood", ct);
            if (totalGamesOwned >= 10) await achievementSystem.TryAwardAsync(userId, "Collector", ct);
            if (totalGamesOwned >= 100) await achievementSystem.TryAwardAsync(userId, "Hoarder", ct);

            //(Fanboy)
            var hasFanboy = userGames
                .Where(ug => ug.Game.PublisherId != null)
                .GroupBy(ug => ug.Game.PublisherId)
                .Any(group => group.Count() >= 3);

            if (hasFanboy) await achievementSystem.TryAwardAsync(userId, "Fanboy", ct);

            //(Genre Explorer)
            var gameIds = userGames.Select(ug => ug.GameId).ToList();
            var uniqueGenresCount = await context.GameGenres
                .Where(gg => gameIds.Contains(gg.GameId))
                .Select(gg => gg.GenreId)
                .Distinct()
                .CountAsync(ct);

            if (uniqueGenresCount >= 5) await achievementSystem.TryAwardAsync(userId, "Genre explorer", ct);

            // (Night Owl)
            var latestPurchase = userGames.OrderByDescending(ug => ug.Id).FirstOrDefault();
            if (latestPurchase is not null)
            {
                var purchaseHour = latestPurchase.CreatedAtUtc.ToLocalTime().Hour;
                if (purchaseHour >= 0 && purchaseHour < 5)
                {
                    await achievementSystem.TryAwardAsync(userId, "Night Owl", ct);
                }
            }
        }

    }
}
