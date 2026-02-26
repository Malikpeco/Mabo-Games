using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Carts.Commands.SwitchItemState
{
    public sealed class SwitchItemStateCommand :IRequest<Unit>
    {
        public int CartItemId { get; set; }
    }
}
