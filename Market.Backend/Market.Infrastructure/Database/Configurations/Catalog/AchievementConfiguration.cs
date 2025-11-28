using Market.Domain.Entities;

namespace Market.Infrastructure.Database.Configurations
{
    public class AchievementConfiguration:IEntityTypeConfiguration<AchievementEntity>
    {

        public void Configure(EntityTypeBuilder<AchievementEntity> builder)
        {
            builder
                .ToTable("Achievements");

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(x => x.Description)
                .HasMaxLength(500);

            builder
                .Property(x => x.ImageURL)
                .IsRequired()
                .HasMaxLength(300);

            builder
                .HasMany(x => x.UserAchievements) //one Achievement can have many UserAchievement entries.
                .WithOne(x => x.Achievement) //each UserAchievement points to one Achievement.
                .HasForeignKey(x => x.AchievementId) //specifies which property in UserAchievementEntity stores the foreign key.
                ; //if an achievement is deleted, all related UserAchievement rows are automatically deleted.
        }
    }
}
