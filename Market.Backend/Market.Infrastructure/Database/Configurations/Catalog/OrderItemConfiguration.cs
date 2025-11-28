using Market.Domain.Entities;

namespace Market.Infrastructure.Database.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItemEntity>
    {
        public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
        {
            builder.ToTable("OrderItems");

            builder.Property(oi => oi.Price)
                   .IsRequired()
                   .HasPrecision(18, 2);

            builder.HasOne(oi => oi.Order)
                   .WithMany(o => o.OrderItems)
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Cascade); 

            builder.HasOne(oi => oi.Game)
                   .WithMany(g => g.OrderItems)
                   .HasForeignKey(oi => oi.GameId)
                   .OnDelete(DeleteBehavior.NoAction);
            // Cascade: deleting an order deletes its items
        }
    }
}
