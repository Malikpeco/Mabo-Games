
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Carts.Commands.Create
{
    public sealed class AddToCartCommand : IRequest<Unit>
    {
        required public int GameId { get; set; }
    }
}
