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
                throw new MarketForbiddenException();


            bool publisherExists = await context.Publishers.AnyAsync(x => x.Id == request.PublisherId);

            if (!publisherExists)
                throw new MarketNotFoundException($"Publisher with id:{request.PublisherId} was not found.");

            
            var game = new GameEntity
            {
                Name = request.Name,
                Price = request.Price,
                Description = request.Description,
                ReleaseDate = request.ReleaseDate,
                CoverImageURL = request.CoverImageURL,
                PublisherId = request.PublisherId 
            };

            // 2. Link existing Genres (Many-to-Many)
            foreach (var genreId in request.GenreIds)
                game.AddGenre(genreId);
            
            

            // 3. Add Screenshots (One-to-Many)
            foreach (var url in request.ScreenshotUrls)
                game.AddScreenshot(url);
            
            

            context.Games.Add(game);
            await context.SaveChangesAsync(ct);

            return game.Id;
        }
    }
}
