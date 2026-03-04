using Market.Domain.Entities;

namespace Market.Infrastructure.Database.Configurations
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItemEntity>
    {
        public void Configure(EntityTypeBuilder<CartItemEntity> builder)
        {

            builder.ToTable("CartItems");


            builder.HasOne(ci => ci.Cart)
                   .WithMany(c => c.CartItems)
                   .HasForeignKey(ci => ci.CartId)
                   .OnDelete(DeleteBehavior.Cascade);   // OK

            builder.HasOne(ci => ci.Game)
                   .WithMany(g => g.CartItems)
                   .HasForeignKey(ci => ci.GameId)
                   .OnDelete(DeleteBehavior.NoAction);


            builder.Property(ci => ci.AddedAt)
                   .IsRequired();

            builder.Property(ci => ci.IsSaved)
                   .IsRequired()
                   .HasDefaultValue(false);

        }

    }
}
