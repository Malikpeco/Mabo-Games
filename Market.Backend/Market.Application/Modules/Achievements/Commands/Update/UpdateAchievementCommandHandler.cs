using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Achievements.Commands.Update
{
    public sealed class UpdateAchievementCommandHandler(IAppCurrentUser currentUser, IAppDbContext context)
        : IRequestHandler<UpdateAchievementCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateAchievementCommand request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this!");

            var ach = await context.Achievements.FirstOrDefaultAsync(a => a.Id == request.Id, ct);
            if (ach is null)
                throw new MarketNotFoundException("Achievement not found!");

            ach.Name = request.Name;
            ach.Description = request.Description;
            ach.ImageURL = request.ImageURL;

            await context.SaveChangesAsync(ct);
            return Unit.Value;

        }
    }
}
