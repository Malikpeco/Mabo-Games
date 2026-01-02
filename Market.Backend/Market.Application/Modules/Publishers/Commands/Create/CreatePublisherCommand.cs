using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Publishers.Commands.Create
{
    public sealed class CreatePublisherCommand : IRequest<int>
    {
        public string Name { get; set; }
        public int CountryId { get; set; }
    }
}
