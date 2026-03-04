using Market.Application.Modules.Carts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Market.Application.Modules.Carts.Queries
{
    public sealed class GetCartByUserIdQuery : IRequest<CartDto>
    {
    }
}
