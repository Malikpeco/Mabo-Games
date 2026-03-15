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
                .HasMany(x => x.UserAchievements) 
                .WithOne(x => x.Achievement) 
                .HasForeignKey(x => x.AchievementId) 
                ; 
        }
    }
}
