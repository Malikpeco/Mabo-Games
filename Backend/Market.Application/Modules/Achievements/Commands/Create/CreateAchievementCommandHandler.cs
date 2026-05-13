using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Achievements.Commands.Create
{
    public sealed class CreateAchievementCommandHandler(IAppCurrentUser currentUser, IAppDbContext context)
        : IRequestHandler<CreateAchievementCommand, Unit>
    {
        public async Task<Unit> Handle(CreateAchievementCommand request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this!");

            var normalizedName = request.Name.Trim();
            var exists = await context.Achievements
                .AnyAsync(a => a.Name.ToLower() == normalizedName.ToLower(), ct);

            if (exists)
                throw new MarketConflictException($"Achievement with name '{normalizedName}' already exists.");


            var newAchievement = new AchievementEntity
            {
                Name = normalizedName,
                Description = request.Description,
                ImageURL = request.ImageURL,
            };

            context.Achievements.Add(newAchievement);
            await context.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
