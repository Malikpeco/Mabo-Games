using Market.Domain.Common;
using Market.Domain.Common.Attributes;
using Market.Domain.Entities.Identity;

namespace Market.Domain.Entities
{
    public class OrderEntity : BaseEntity
    {
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }

        [PreserveString]
        public string OrderStatus { get; set; }
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        public PaymentEntity Payment { get; set; }
        public IReadOnlyCollection<OrderItemEntity> OrderItems { get; private set; } = new List<OrderItemEntity>();

        public OrderEntity()
        {
            Date = DateTime.UtcNow;  // timestamp when created
        }
    }
}
