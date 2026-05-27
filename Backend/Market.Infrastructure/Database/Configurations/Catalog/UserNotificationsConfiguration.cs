using Market.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.Database.Configurations.Catalog
{
    internal class UserNotificationsConfiguration : IEntityTypeConfiguration<UserNotificationsEntity>
    {
        public void Configure(EntityTypeBuilder<UserNotificationsEntity> builder)
        {
            builder.ToTable("UserNotifications");


            builder.HasIndex(un => new { un.UserId, un.NotificationId }).IsUnique();


            builder.HasOne(u => u.User)
                   .WithMany(un => un.UserNotifications)
                   .HasForeignKey(usq => usq.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(n => n.Notification)
                   .WithMany(u => u.UserNotifications)
                   .HasForeignKey(usq => usq.NotificationId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.Property(u => u.IsRead)
                .HasDefaultValue(false)
                .IsRequired();
        }
    }

}
