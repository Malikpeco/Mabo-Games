using Market.Domain.Common;
using Market.Domain.Common.Attributes;

namespace Market.Domain.Entities
{
    public class GenreEntity : BaseEntity
    {
        [PreserveCapitalization]
        public string Name { get; set; }
        public IReadOnlyCollection<GameGenreEntity> GameGenres { get; private set; }
    }
}
