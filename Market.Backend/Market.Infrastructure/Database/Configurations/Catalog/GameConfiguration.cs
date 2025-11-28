using Market.Domain.Entities;

namespace Market.Infrastructure.Database.Configurations
{
    public class GameConfiguration : IEntityTypeConfiguration<GameEntity>
    {
        public void Configure(EntityTypeBuilder<GameEntity> builder)
        {
            builder.ToTable("Games");

            builder.Property(g => g.Name)
                   .IsRequired()
                   .HasMaxLength(300);

            
            builder.Property(g => g.Price)
                   .IsRequired()
                   .HasPrecision(18, 2);

            
            builder.Property(g => g.Description)
                   .HasMaxLength(1000);

            
            builder.Property(g => g.ReleaseDate)
                   .IsRequired();

            
            builder.HasOne(g => g.Publisher)
                   .WithMany(p => p.Games)
                   .HasForeignKey(g => g.PublisherId)
                   .IsRequired()
                   ; 



            builder.HasMany(g => g.CartItems)
                   .WithOne(ci => ci.Game)
                   .HasForeignKey(ci => ci.GameId)
                   ; 


            builder.HasMany(g => g.Favourites)
                   .WithOne(f => f.Game)
                   .HasForeignKey(f => f.GameId)
                   ;


            builder.HasMany(g => g.UserGames)
                   .WithOne(ug => ug.Game)
                   .HasForeignKey(ug => ug.GameId)
                   .OnDelete(DeleteBehavior.NoAction);


            builder.HasMany(g => g.GameGenres)
                   .WithOne(gg => gg.Game)
                   .HasForeignKey(gg => gg.GameId)
                   ;


            builder.HasMany(g => g.OrderItems)
                   .WithOne(oi => oi.Game)
                   .HasForeignKey(oi => oi.GameId)
                   ;


            builder.HasMany(g => g.Screenshots)
                   .WithOne(s => s.Game)
                   .HasForeignKey(s => s.GameId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
