using Market.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Domain.Entities
{
    public class CartEntity : BaseEntity
    {
        public UserEntity User { get; set; }
        public int UserId { get; set; }
        public IReadOnlyCollection<CartItemEntity> CartItems { get; private set; } = new List<CartItemEntity>();
    }
}
