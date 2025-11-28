using Market.Domain.Common;

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

    }
}
