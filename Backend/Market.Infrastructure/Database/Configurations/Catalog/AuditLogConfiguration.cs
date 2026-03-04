using Market.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market.Infrastructure.Database.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLogEntity>
    {
        public void Configure(EntityTypeBuilder<AuditLogEntity> builder)
        {
            builder.ToTable("AuditLogs");

            builder.Property(x => x.EntityName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Action)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(x => x.EntityId)
                   .HasMaxLength(64);

            builder.Property(x => x.BeforeChange)
                   .HasColumnType("nvarchar(max)");

            builder.Property(x => x.AfterChange)
                   .HasColumnType("nvarchar(max)");

            builder.HasIndex(x => x.EntityName);
            builder.HasIndex(x => x.EntityId);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.CreatedAtUtc);
        }
    }
}
