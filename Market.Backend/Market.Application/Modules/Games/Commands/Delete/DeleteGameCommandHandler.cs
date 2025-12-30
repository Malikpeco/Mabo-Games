using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Games.Commands.Delete
{
    public sealed class DeleteGameCommandHandler(IAppDbContext context, IAppCurrentUser currentUser) : IRequestHandler<DeleteGameCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteGameCommand request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new MarketForbiddenException();

            var game = await context.Games.FirstOrDefaultAsync(g=>g.Id==request.Id, ct);
            if (game is null)
                throw new MarketNotFoundException("Game doesnt exist");

            context.Games.Remove(game);
            await context.SaveChangesAsync(ct);

            return Unit.Value;
        }

    }
}
