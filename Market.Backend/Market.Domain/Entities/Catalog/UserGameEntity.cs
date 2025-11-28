using Market.Domain.Common;
using Market.Domain.Entities.Identity;

namespace Market.Domain.Entities
    {
        public class UserGameEntity : BaseEntity
        {
            public int UserId { get; set; }
            public UserEntity User { get; set; }
            public int GameId { get; set; }
            public GameEntity Game { get; set; }
            public DateTime PurchaseDate { get; set; }
            public ReviewEntity? Review { get; set; }

            public UserGameEntity()
            {
                PurchaseDate= DateTime.UtcNow;
            }
        }
    }
