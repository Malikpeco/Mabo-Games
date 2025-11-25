using Market.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Domain.Entities
{
    public class CountryEntity : BaseEntity
    {
        public string Name { get; set; }
        public IReadOnlyCollection<CityEntity> Cities { get; private set; } = new List<CityEntity>();
        public IReadOnlyCollection<PublisherEntity> Publishers { get; private set; } = new List<PublisherEntity>();

    }
}
