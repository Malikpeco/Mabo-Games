using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Carts.Dto
{
    public sealed class CartDto
    {
        public int Id { get; set; }
        public List<CartItemDto> CartItems { get; set; } = new();
        public decimal TotalPrice { get; set; }
    }
}
