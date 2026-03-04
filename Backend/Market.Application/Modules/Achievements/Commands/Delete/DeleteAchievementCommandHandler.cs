using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Achievements.Commands.Delete
{
    public sealed class DeleteAchievementCommandHandler(IAppCurrentUser currentUser, IAppDbContext context)
        : IRequestHandler<DeleteAchievementCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteAchievementCommand request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this!");

            var ach = await context.Achievements.FirstOrDefaultAsync(a => a.Id == request.Id, ct);

            if (ach is null)
                throw new MarketNotFoundException("Achievement not found");

            context.Achievements.Remove(ach);
            await context.SaveChangesAsync(ct);
            return Unit.Value;

        }
    }
}
