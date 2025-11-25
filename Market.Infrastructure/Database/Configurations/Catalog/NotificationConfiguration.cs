using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.Database.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<NotificationEntity>
    {
        public void Configure(EntityTypeBuilder<NotificationEntity> builder)
        {
            builder.ToTable("Notifications");

            builder.Property(n => n.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(n => n.Content)
                   .IsRequired()
                   .HasMaxLength(2000);

            builder.Property(n => n.IsRead)
                   .IsRequired();

            
            builder.Property(n => n.SentAt)
                   .IsRequired();

            builder.HasOne(n => n.User)
                   .WithMany(u => u.Notifications) 
                   .HasForeignKey(n => n.UserId)
                   .IsRequired()
                   ;
        }
    }
}
