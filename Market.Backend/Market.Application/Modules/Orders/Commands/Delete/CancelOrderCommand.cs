using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Orders.Commands.Delete
{
    public sealed class CancelOrderCommand : IRequest<Unit>
    {
        public int OrderId { get; set; }
    }
}
