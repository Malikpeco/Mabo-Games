using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Countries.Commands.Update
{
    public sealed class UpdateCountryCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
