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
                throw new Exception("You must be an admin to do this!");


            var game = await context.Games.FirstOrDefaultAsync(g=>g.Id==request.Id, ct);
            if (game is null)
                throw new MarketNotFoundException("Game doesnt exist");

            if (await context.UserGames.AnyAsync(ug => ug.GameId == game.Id, ct))
                throw new MarketBusinessRuleException("422", "Cannot delete a game that a user owns. Delete the UserGame first.");


            context.Games.Remove(game);
            await context.SaveChangesAsync(ct);

            return Unit.Value;
        }

    }
}
