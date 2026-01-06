using Market.Application.Modules.Cities.Queries.Dto;
using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Cities.Queries.GetById
{
    public sealed class GetCityByIdQueryDto 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public CountryDto Country { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public string PostalCode { get; set; }

        public IReadOnlyList<UserDto> Users { get; set; }
    }

    public sealed class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }


}
