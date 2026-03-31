namespace Market.Application.Modules.Genres.Commands.Delete
{
    public sealed class DeleteGenreCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<DeleteGenreCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteGenreCommand request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this.");

            var genre = await context.Genres.FirstOrDefaultAsync(g => g.Id == request.Id, ct);
            if (genre is null)
                throw new MarketNotFoundException("Genre not found.");

            if (await context.GameGenres.AnyAsync(gg => gg.GenreId == request.Id, ct))
                throw new MarketBusinessRuleException("400", "Cannot delete genre because it has games assigned.");

            context.Genres.Remove(genre);
            await context.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}
