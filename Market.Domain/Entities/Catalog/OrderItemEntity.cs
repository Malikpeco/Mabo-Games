using Market.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
