using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.Database.Configurations
{
    public class UserGameConfiguration : IEntityTypeConfiguration<UserGameEntity>
    {
        public void Configure(EntityTypeBuilder<UserGameEntity> builder)
        {
            // Composite key ensures unique User + Game pair
            builder.HasKey(ug => new { ug.UserId, ug.GameId });


            builder.HasOne(ug => ug.User)
                   .WithMany(u => u.UserGames)
                   .HasForeignKey(ug => ug.UserId)
                   ;

            builder.HasOne(ug => ug.Game)
                   .WithMany(g => g.UserGames)
                   .HasForeignKey(ug => ug.GameId)
                   ;

            builder.HasOne(ug => ug.Review)
                   .WithOne(r => r.UserGame)
                   .HasForeignKey<ReviewEntity>(r => new { r.UserId, r.GameId })
                   ;
        }
    }
}
