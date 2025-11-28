using Market.Domain.Entities;

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
