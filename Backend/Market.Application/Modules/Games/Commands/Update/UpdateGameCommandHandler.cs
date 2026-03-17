using Market.Domain.Entities;

namespace Market.Application.Modules.Games.Commands.Update
{
    public sealed class UpdateGameCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<UpdateGameCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateGameCommand request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new UnauthorizedAccessException("You must be an admin to do this!");


            var game = await context.Games
                .Include(g => g.GameGenres)
                .Include(g => g.Screenshots)
                .FirstOrDefaultAsync(g => g.Id == request.Id, ct);

            if (game is null)
                throw new MarketNotFoundException("Game not found");

            var publisher = await context.Publishers
                .FirstOrDefaultAsync(p => p.Id == request.PublisherId, ct);

            if (publisher is null)
                throw new MarketNotFoundException("Publisher not found");


            game.Name = request.Name;
            game.Description = request.Description;
            game.Price = request.Price;
            game.ReleaseDate = request.ReleaseDate;
            game.CoverImageURL = request.CoverImageURL;
            game.PublisherId = request.PublisherId;


            var genreIds = request.GenreIds
                .Where(id => id > 0)
                .Distinct()
                .ToList();

            var genresList = (List<GameGenreEntity>)game.GameGenres;
            genresList.Clear();

            foreach (var genreId in genreIds)
            {
                genresList.Add(new GameGenreEntity
                {
                    GameId = game.Id,
                    GenreId = genreId
                });
            }

            var screenshotUrls = request.ScreenshotUrls
                .Where(url => !string.IsNullOrWhiteSpace(url))
                .Select(url => url.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var screenshotsList = (List<ScreenshotEntity>)game.Screenshots;
            screenshotsList.Clear();

            foreach (var url in screenshotUrls)
            {
                screenshotsList.Add(new ScreenshotEntity
                {
                    GameId = game.Id,
                    ImageURL = url
                });
            }

            await context.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}