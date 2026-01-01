using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Cities.Commands.Update
{
    public sealed class UpdateCityCommand : IRequest<int>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public string PostalCode { get; set; }
    }
}
