using Market.Domain.Entities;

namespace Market.Infrastructure.Database.Configurations
{
    public class UserAchievementConfiguration : IEntityTypeConfiguration<UserAchievementEntity>
    {
        public void Configure(EntityTypeBuilder<UserAchievementEntity> builder)
        {
            builder.ToTable("UserAchievements");

            builder.Property(ua => ua.AchievedAt)
                   .IsRequired();

            builder.HasOne(ua => ua.User)
                   .WithMany(u => u.UserAchievements)
                   .HasForeignKey(ua => ua.UserId)
                   .IsRequired()
                   ;


            builder.HasOne(ua => ua.Achievement)
                   .WithMany(a => a.UserAchievements)
                   .HasForeignKey(ua => ua.AchievementId)
                   .IsRequired()
                   ;


            builder.HasIndex(ua => new { ua.UserId, ua.AchievementId })
                   .IsUnique();
        }
    }
}
