    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Cities.Commands.Delete
{
    public sealed class DeleteCityCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
