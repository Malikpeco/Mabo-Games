using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                   .HasForeignKey<ReviewEntity>(r => new { r.UserId, r.GameId })
                   .IsRequired()
                   ;
            // Cascade: deleting UserGame deletes the review
        }
    }
}
