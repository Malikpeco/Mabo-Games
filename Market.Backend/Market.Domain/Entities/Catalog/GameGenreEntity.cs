using Market.Domain.Common;
using Market.Domain.Common.Attributes;

namespace Market.Domain.Entities
{

   
    public class GameGenreEntity : BaseEntity
    {
        public int GameId { get; set; }
        public GameEntity Game { get; set; }
        public int GenreId { get; set; }
        public GenreEntity Genre { get; set; }
    }
}
