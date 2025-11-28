using Market.Domain.Entities;

namespace Market.Infrastructure.Database.Configurations
{
    public class CityConfiguration : IEntityTypeConfiguration<CityEntity>
    {
        public void Configure(EntityTypeBuilder<CityEntity> builder)
        {
            builder.ToTable("Cities");

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100); 

            builder.Property(c => c.PostalCode)
                   .IsRequired()
                   .HasMaxLength(30);

            builder.Property(c => c.Longitude)
                   .IsRequired();

            builder.Property(c => c.Latitude)
                   .IsRequired();

            builder.HasOne(c => c.Country)
                   .WithMany(co => co.Cities)
                   .HasForeignKey(c => c.CountryId)
                   .IsRequired()       
                   ;
        }
    }
}
