using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                   ;
            // Restrict: can't delete a country if it has publishers

            builder.HasMany(p => p.Games)
                   .WithOne(g => g.Publisher)
                   .HasForeignKey(g => g.PublisherId)
                   .IsRequired()
                   ;
            // Restrict: can't delete a publisher if it has games
        }
    }
}
