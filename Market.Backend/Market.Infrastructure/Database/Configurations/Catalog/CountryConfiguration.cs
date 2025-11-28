using Market.Domain.Entities;

namespace Market.Infrastructure.Database.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<CountryEntity>
    {
        public void Configure(EntityTypeBuilder<CountryEntity> builder)
        {
            builder.ToTable("Countries");

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            
            builder.HasMany(c => c.Cities)
                   .WithOne(city => city.Country)
                   .HasForeignKey(city => city.CountryId)
                   .IsRequired()
                   ; 

            builder.HasMany(c => c.Publishers)
                   .WithOne(pub => pub.Country)
                   .HasForeignKey(pub => pub.CountryId)
                   .IsRequired()
                   ;

        }
    }
}
