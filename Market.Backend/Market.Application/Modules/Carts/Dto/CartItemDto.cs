using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Carts.Dto
{
    public sealed class CartItemDto
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; }
        public decimal Price { get; set; }
        public DateTime AddedAt { get; set; }
        public bool IsSaved { get; set; }
    }
}
