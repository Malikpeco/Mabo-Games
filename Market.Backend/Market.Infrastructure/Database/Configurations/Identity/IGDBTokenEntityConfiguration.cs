using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.Database.Configurations.Identity
{
    public class IGDBTokenEntityConfiguration : IEntityTypeConfiguration<IGDBTokenEntity>
    {
        public void Configure(EntityTypeBuilder<IGDBTokenEntity> b)
        {
            b.ToTable("IGDBTokens");
            b.HasKey(x => x.Id);
            b.Property(x => x.Token)
                .IsRequired()
                .HasMaxLength(256);
            b.Property(x => x.ExpiresAt)
                .IsRequired();

            b.Property(x => x.LastUpdated)
                .IsRequired();

        }
    }
}
