namespace Market.Application.Modules.Genres.Commands.Delete
{
    public sealed class DeleteGenreCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
