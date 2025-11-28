using Market.Domain.Entities;

namespace Market.Infrastructure.Database.Configurations
{
    public class ScreenshotConfiguration : IEntityTypeConfiguration<ScreenshotEntity>
    {
        public void Configure(EntityTypeBuilder<ScreenshotEntity> builder)
        {
            builder.ToTable("Screenshots");

            builder.Property(s => s.ImageURL)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.HasOne(s => s.Game)
                   .WithMany(g => g.Screenshots)
                   .HasForeignKey(s => s.GameId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade)
                   ;
        }
    }
}
