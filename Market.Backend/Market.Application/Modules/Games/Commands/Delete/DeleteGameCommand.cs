using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Games.Commands.Delete
{
    public sealed class DeleteGameCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
