using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Publishers.Commands.Update
{
    public sealed class UpdatePublisherCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int Id { get; set; } 
        public string Name { get; set; }
        public int CountryId { get; set; }
    }
}
