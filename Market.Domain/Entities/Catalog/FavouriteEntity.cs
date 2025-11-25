using Market.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
