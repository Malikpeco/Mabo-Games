using Market.Domain.Entities;

namespace Market.Infrastructure.Database.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<ReviewEntity>
    {
        public void Configure(EntityTypeBuilder<ReviewEntity> builder)
        {
            // Table name
            builder.ToTable("Reviews");

            builder.Property(r => r.Content)
                   .HasMaxLength(2000);

            builder.Property(r => r.Rating)
          .IsRequired();

            builder.Property(r => r.Date)
                   .IsRequired();

            builder.HasOne(r => r.UserGame)
                   .WithOne(ug => ug.Review)
                   .HasForeignKey<ReviewEntity>(r => r.UserGameId)
                   .OnDelete(DeleteBehavior.Cascade);
            // Cascade: deleting UserGame deletes the review

        }
    }
}
