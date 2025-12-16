using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Carts.Commands.Delete
{
    public sealed class RemoveFromCartCommand : IRequest<Unit>
    {
        public int GameId { get; set; }
    }
}
