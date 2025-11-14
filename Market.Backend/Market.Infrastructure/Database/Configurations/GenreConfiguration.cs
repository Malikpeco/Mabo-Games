using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.Database.Configurations
{
    public class GenreConfiguration : IEntityTypeConfiguration<GenreEntity>
    {
        public void Configure(EntityTypeBuilder<GenreEntity> builder)
        {
            
            builder.ToTable("Genres");

            
            builder.Property(g => g.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            
            builder.HasMany(g => g.GameGenres)
                   .WithOne(gg => gg.Genre)
                   .HasForeignKey(gg => gg.GenreId)
                   ;
        }
    }
}
