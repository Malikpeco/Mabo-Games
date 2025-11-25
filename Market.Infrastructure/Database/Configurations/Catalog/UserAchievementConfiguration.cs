using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // Cascade: deleting a user deletes their achievements links

            builder.HasOne(ua => ua.Achievement)
                   .WithMany(a => a.UserAchievements)
                   .HasForeignKey(ua => ua.AchievementId)
                   .IsRequired()
                   ;
            // Cascade: deleting an achievement deletes its user links

            // prevent duplicate achievements for the same user
            builder.HasIndex(ua => new { ua.UserId, ua.AchievementId })
                   .IsUnique();
        }
    }
}
