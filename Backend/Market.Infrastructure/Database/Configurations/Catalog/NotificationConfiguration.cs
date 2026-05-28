using Market.Domain.Entities;

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

            builder.Property(n => n.SentAt)
                   .IsRequired();

            builder
            .HasMany(x => x.UserNotifications)
            .WithOne(x => x.Notification)
            .HasForeignKey(x => x.NotificationId)
            ;




        }
    }
}
