using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.Database.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("Users");

            builder.Property(u => u.Username)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.PasswordHash)
                   .IsRequired();

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(u => u.FirstName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.LastName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.ProfileImageURL)
                   .HasMaxLength(500);

            builder.Property(u => u.ProfileBio)
                   .HasMaxLength(1000);

            builder.Property(u => u.CreationDate)
                   .IsRequired();

            builder.Property(u => u.IsAdmin)
                   .IsRequired();

            
            builder.HasOne(u => u.City)
                   .WithMany(c=>c.Users)
                   .HasForeignKey(u => u.CityId)
                   .IsRequired()
                   ;
            // Restrict: cannot delete a city if users exist

    
            builder.HasOne(u => u.Cart)
                   .WithOne(c => c.User)
                   .HasForeignKey<CartEntity>(c => c.UserId)
                   .IsRequired()
                   ;
            // Cascade: deleting user deletes their cart


            builder.HasMany(u => u.UserGames)
                   .WithOne(ug => ug.User)
                   .HasForeignKey(ug => ug.UserId)
                   .IsRequired()
                   ;

            
            // Relationship: User → UserAchievements (one-to-many)
            builder.HasMany(u => u.UserAchievements)
                   .WithOne(ua => ua.User)
                   .HasForeignKey(ua => ua.UserId)
                   .IsRequired()
                   ;

            // Relationship: User → Favourites (one-to-many)
            builder.HasMany(u => u.Favourites)
                   .WithOne(f => f.User)
                   .HasForeignKey(f => f.UserId)
                   .IsRequired();

            // Relationship: User → Notifications (one-to-many)
            builder.HasMany(u => u.Notifications)
                   .WithOne(n => n.User)
                   .HasForeignKey(n => n.UserId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.NoAction);

            // Relationship: User → Orders (one-to-many)
            builder.HasMany(u => u.Orders)
                   .WithOne(o => o.User)
                   .HasForeignKey(o => o.UserId)
                   .IsRequired()
                   ;

            // Relationship: User → UserSecurityQuestions (one-to-many)
            builder.HasMany(u => u.UserSecurityQuestions)
                   .WithOne(usq => usq.User)
                   .HasForeignKey(usq => usq.UserId)
                   .IsRequired()
                   ;
        }
    }
}
