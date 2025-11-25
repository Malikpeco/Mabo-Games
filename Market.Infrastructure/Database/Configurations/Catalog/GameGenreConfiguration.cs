using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.Database.Configurations
{
    public class GameGenreConfiguration : IEntityTypeConfiguration<GameGenreEntity>
    {
        public void Configure(EntityTypeBuilder<GameGenreEntity> builder)
        {

            builder.ToTable("GameGenres");

            builder.HasOne(gg => gg.Game)
                   .WithMany(g => g.GameGenres)
                   .HasForeignKey(gg => gg.GameId)
                   .IsRequired()
                   ; 

            
            builder.HasOne(gg => gg.Genre)
                   .WithMany(g => g.GameGenres)
                   .HasForeignKey(gg => gg.GenreId)
                   .IsRequired()
                   ; 

            //  unique constraint to prevent duplicate game-genre pairs
            builder.HasIndex(gg => new { gg.GameId, gg.GenreId })
                   .IsUnique();
        }
    }
}
