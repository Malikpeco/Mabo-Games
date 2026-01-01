using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Countries.Commands.Delete
{
    public sealed class DeleteCountryCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
