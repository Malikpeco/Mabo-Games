using Market.Domain.Entities;

namespace Market.Infrastructure.Database.Configurations
{
    public class PublisherConfiguration : IEntityTypeConfiguration<PublisherEntity>
    {
        public void Configure(EntityTypeBuilder<PublisherEntity> builder)
        {
            builder.ToTable("Publishers");

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasOne(p => p.Country)
                   .WithMany(c => c.Publishers)
                   .HasForeignKey(p => p.CountryId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.NoAction)
                   ;
            

            builder.HasMany(p => p.Games)
                   .WithOne(g => g.Publisher)
                   .HasForeignKey(g => g.PublisherId)
                   .IsRequired()
                   ;
            
        }
    }
}
