using Market.Domain.Entities;

namespace Market.Infrastructure.Database.Configurations
{
    public class FavouriteConfiguration : IEntityTypeConfiguration<FavouriteEntity>
    {
        public void Configure(EntityTypeBuilder<FavouriteEntity> builder)
        {
            builder.ToTable("Favourites");

            builder.Property(f => f.FavouritedAt)
                   .IsRequired();

            builder.HasOne(f => f.User)
                   .WithMany(u => u.Favourites)
                   .HasForeignKey(f => f.UserId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);



            builder.HasOne(f => f.Game)
                   .WithMany(g => g.Favourites)
                   .HasForeignKey(f => f.GameId)
                   .IsRequired()
                   ; 
        }
    }
}
