using Market.Domain.Common;
using Market.Domain.Entities.Identity;

namespace Market.Domain.Entities
{
    public class CountryEntity : BaseEntity
    {
        public string Name { get; set; }
        public IReadOnlyCollection<CityEntity> Cities { get; private set; } = new List<CityEntity>();
        public IReadOnlyCollection<PublisherEntity> Publishers { get; private set; } = new List<PublisherEntity>();
        public IReadOnlyCollection<UserEntity> Users { get; private set; } = new List<UserEntity>();

    }
}
