using Market.Domain.Entities;

namespace Market.Infrastructure.Database.Configurations
{
    public class UserGameConfiguration : IEntityTypeConfiguration<UserGameEntity>
    {
        public void Configure(EntityTypeBuilder<UserGameEntity> builder)
        {
            
            builder.HasKey(ug => ug.Id);
            builder.HasIndex(ug => new { ug.UserId, ug.GameId }).IsUnique();


            builder.HasOne(ug => ug.User)
                   .WithMany(u => u.UserGames)
                   .HasForeignKey(ug => ug.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ug => ug.Game)
                   .WithMany(g => g.UserGames)
                   .HasForeignKey(ug => ug.GameId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ug => ug.Review)
                   .WithOne(r => r.UserGame)
                   .HasForeignKey<ReviewEntity>(r => r.UserGameId);
            ;
        }
    }
}
