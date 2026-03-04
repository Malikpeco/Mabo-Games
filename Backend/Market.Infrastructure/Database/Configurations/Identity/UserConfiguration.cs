using Market.Domain.Entities;

namespace Market.Infrastructure.Database.Configurations.Identity;

public sealed class UserConfiguration : IEntityTypeConfiguration<UserEntity>
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

        builder.HasOne(u => u.Country)
               .WithMany(c => c.Users)
               .HasForeignKey(u => u.CountryId)
               .OnDelete(DeleteBehavior.NoAction);
               ;

        builder.HasOne(u => u.City)
               .WithMany(c => c.Users)
               .HasForeignKey(u => u.CityId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.NoAction)
               ;
        
        builder.HasOne(u => u.Cart)
               .WithOne(c => c.User)
               .HasForeignKey<CartEntity>(c => c.UserId)
               .OnDelete(DeleteBehavior.Cascade);



        builder.HasMany(u => u.UserGames)
               .WithOne(ug => ug.User)
               .HasForeignKey(ug => ug.UserId)
               .OnDelete(DeleteBehavior.NoAction);



        builder.HasMany(u => u.UserAchievements)
               .WithOne(ua => ua.User)
               .HasForeignKey(ua => ua.UserId)
               .IsRequired()
               ;


        builder.HasMany(u => u.Favourites)
               .WithOne(f => f.User)
               .HasForeignKey(f => f.UserId)
               .IsRequired();

        builder.HasMany(u => u.Notifications)
               .WithOne(n => n.User)
               .HasForeignKey(n => n.UserId)
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);


        builder.HasMany(u => u.Orders)
               .WithOne(o => o.User)
               .HasForeignKey(o => o.UserId)
               .IsRequired()
               ;

        builder.HasMany(u => u.UserSecurityQuestions)
               .WithOne(usq => usq.User)
               .HasForeignKey(usq => usq.UserId)
               .IsRequired()
               ;


        builder.Property(x => x.IsAdmin)
            .HasDefaultValue(false);

        builder.Property(x => x.TokenVersion)
            .HasDefaultValue(0);

        builder.Property(x => x.IsEnabled)
            .HasDefaultValue(true);

     
        builder.HasMany(x => x.RefreshTokens)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
    }
}