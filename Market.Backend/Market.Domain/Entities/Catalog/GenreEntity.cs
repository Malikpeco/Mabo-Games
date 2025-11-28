using Market.Domain.Common;

namespace Market.Domain.Entities
{
    public class GenreEntity : BaseEntity
    {
        public string Name { get; set; }
        public IReadOnlyCollection<GameGenreEntity> GameGenres { get; private set; }
    }
}
