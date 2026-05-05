namespace Market.Application.Modules.Genres.Commands.Update
{
    public sealed class UpdateGenreCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
