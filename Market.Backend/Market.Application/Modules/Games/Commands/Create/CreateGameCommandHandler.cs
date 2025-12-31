using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Games.Commands.Create
{
    public sealed class CreateGameCommandHandler(IAppDbContext context, IAppCurrentUser currentUser) : IRequestHandler<CreateGameCommand, int>
    {
        public async Task<int> Handle(CreateGameCommand request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this!");


            var publisherExists = await context.Publishers.AnyAsync(p => p.Id == request.PublisherId, ct);

            if (!publisherExists)
                throw new Exception("Publisher not found");

            var game = new GameEntity
            {
                Name = request.Name,
                Price = request.Price,
                Description = request.Description,
                ReleaseDate = request.ReleaseDate,
                PublisherId = request.PublisherId,
                CoverImageURL = request.CoverImageURL,
            };

            context.Games.Add(game);
            await context.SaveChangesAsync(ct);

            return game.Id;
        }
    }
}
