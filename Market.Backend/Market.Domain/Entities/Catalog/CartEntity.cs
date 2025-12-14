using Market.Domain.Common;
using Market.Domain.Entities.Identity;

namespace Market.Domain.Entities
{
    public class CartEntity : BaseEntity
    {
        public UserEntity User { get; set; }
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public IReadOnlyCollection<CartItemEntity> CartItems { get; private set; } = new List<CartItemEntity>();

    }
}
