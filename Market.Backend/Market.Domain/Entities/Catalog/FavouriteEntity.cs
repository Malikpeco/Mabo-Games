using Market.Domain.Common;
using Market.Domain.Entities.Identity;

namespace Market.Domain.Entities
{
    public class FavouriteEntity : BaseEntity
    {
        public FavouriteEntity()
        {
            FavouritedAt = DateTime.UtcNow; // automatically set when created
        }
        public DateTime FavouritedAt { get; set; }
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        public int GameId { get; set; }
        public GameEntity Game { get; set; }
    }
}
