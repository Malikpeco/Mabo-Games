using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Games.Commands.Update
{
    public sealed class UpdateGameCommandHandler(IAppDbContext context, IAppCurrentUser currentUser) : IRequestHandler<UpdateGameCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateGameCommand request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new MarketForbiddenException();

            var game = await context.Games.FirstOrDefaultAsync(g=>g.Id == request.Id, ct);
            if (game is null)
                throw new MarketNotFoundException("Game not found");

            
            var publisher = await context.Publishers.FirstOrDefaultAsync(p=>p.Id == request.PublisherId, ct);
            if (publisher is null)
                throw new MarketNotFoundException("Publisher not found");

            game.Name = request.Name;
            game.Description = request.Description;
            game.Price = request.Price;
            game.ReleaseDate = request.ReleaseDate;
            game.CoverImageURL = request.CoverImageURL;
            game.PublisherId = request.PublisherId;

            await context.SaveChangesAsync(ct);
            return Unit.Value;


        }
    }
}
