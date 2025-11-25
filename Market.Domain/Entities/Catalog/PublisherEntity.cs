using Market.Domain.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Domain.Entities
{
    public class PublisherEntity : BaseEntity
    {
        public string Name { get; set; }
        public int CountryId { get; set; }
        public CountryEntity Country { get; set; }
        public IReadOnlyCollection<GameEntity> Games { get; private set; } = new List<GameEntity>();
    }
}
