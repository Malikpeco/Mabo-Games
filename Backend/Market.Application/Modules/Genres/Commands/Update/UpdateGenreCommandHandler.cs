namespace Market.Application.Modules.Genres.Commands.Update
{
    public sealed class UpdateGenreCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<UpdateGenreCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateGenreCommand request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this.");

            var genre = await context.Genres.FirstOrDefaultAsync(g => g.Id == request.Id, ct);
            if (genre is null)
                throw new MarketNotFoundException("Genre not found.");

            bool nameTaken = await context.Genres
                .AnyAsync(g => g.Id != request.Id && g.Name.ToLower() == request.Name.ToLower(), ct);

            if (nameTaken)
                throw new MarketConflictException($"Genres already contain: {request.Name}!");

            genre.Name = request.Name;

            await context.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
