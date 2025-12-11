using Market.Application.Modules.RegisterUser.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.AddToCart.Commands
{
    public sealed class AddToCartCommand : IRequest<Unit>
    {
        required public int GameId { get; set; }
    }
}
