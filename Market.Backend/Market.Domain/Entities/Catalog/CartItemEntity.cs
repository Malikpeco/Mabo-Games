using Market.Domain.Common;

namespace Market.Domain.Entities
{
    public class CartItemEntity : BaseEntity
    {
        public CartItemEntity()
        {
            AddedAt = DateTime.UtcNow;
            IsSaved = false; // default: not saved
        }

        public CartEntity Cart { get; set; }
        public int CartId { get; set; }
        public GameEntity Game { get; set; }
        public int GameId { get; set; }
        public DateTime AddedAt { get; set; }
        public bool IsSaved { get; set; }
    }
}
