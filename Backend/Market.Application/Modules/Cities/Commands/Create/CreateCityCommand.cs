using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Cities.Commands.Create
{
    public sealed class CreateCityCommand : IRequest<int>
    {
        public string Name { get; set; }
        public int CountryId { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public string PostalCode { get; set; }
    }
}
