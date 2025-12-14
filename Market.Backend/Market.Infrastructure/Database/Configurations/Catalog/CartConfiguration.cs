using Market.Domain.Entities;

namespace Market.Infrastructure.Database.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<CartEntity>
    {
        public void Configure(EntityTypeBuilder<CartEntity> builder) 
        {
            builder
               .ToTable("Carts");


            builder
                .HasOne(c => c.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<CartEntity>(c => c.UserId)
                ; // delete cart if the user is deleted

            builder
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId)
                ; //delete all cartItems if cart is deleted


            builder.Property(c => c.TotalPrice)
            .HasColumnType("decimal(18,2)");
        }
    }
}
