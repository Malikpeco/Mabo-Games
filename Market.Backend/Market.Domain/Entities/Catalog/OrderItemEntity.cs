using Market.Domain.Common;

namespace Market.Domain.Entities
{
    public class OrderItemEntity : BaseEntity
    {
        public Decimal Price { get; set; }
        public int GameId { get; set; }
        public GameEntity Game { get; set; }
        public int OrderId { get; set; }
        public OrderEntity Order { get; set; }

    }
}
