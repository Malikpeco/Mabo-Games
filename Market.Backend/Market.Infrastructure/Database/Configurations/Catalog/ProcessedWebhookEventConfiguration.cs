using Market.Domain.Entities;
using Market.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.Database.Configurations.Catalog
{
    public class ProcessedWebhookEventConfiguration : IEntityTypeConfiguration<ProcessedWebhookEventEntity>
    {
        public void Configure(EntityTypeBuilder<ProcessedWebhookEventEntity> builder)
        {
            builder.ToTable("ProcessedWebhookEvents");

            builder.Property(x => x.Provider)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.EventId)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.EventType)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(x => x.EventId)
                   .IsUnique();
        }
    }
}
