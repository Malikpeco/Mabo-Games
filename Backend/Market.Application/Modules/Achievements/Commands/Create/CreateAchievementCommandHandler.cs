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


            var newAchievement = new AchievementEntity
            {
                Name = request.Name,
                Description = request.Description,
                ImageURL = request.ImageURL,
            };

            context.Achievements.Add(newAchievement);
            await context.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
