using Market.Application.Modules.Cities.Queries.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Cities.Queries.List
{
    public sealed class ListCitiesQueryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CountryDto Country { get; set; }
    }

}
