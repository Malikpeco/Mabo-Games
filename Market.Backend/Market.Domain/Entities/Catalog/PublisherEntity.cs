using Market.Domain.Common;
using Market.Domain.Common.Attributes;

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
