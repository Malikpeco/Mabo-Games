using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Publishers.Commands.Delete
{
    public sealed class DeletePublisherCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
