using Market.Domain.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Domain.Entities
{
    public class CityEntity : BaseEntity
    {
        public string Name { get; set; }
        public int CountryId { get; set; }
        public CountryEntity Country { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public string PostalCode { get; set; }
        public IReadOnlyCollection<UserEntity> Users { get; private set; } = new List<UserEntity>();
    }
}
