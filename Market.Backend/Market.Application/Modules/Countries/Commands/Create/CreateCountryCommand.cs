using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Countries.Commands.Create
{
    public class CreateCountryCommand : IRequest<int>
    {
        public required string Name { get; set; }
        
    }
}
