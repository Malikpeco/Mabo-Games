using Market.Domain.Entities;

namespace Market.Infrastructure.Database.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.ToTable("Orders");

            builder.Property(o => o.Date)
                   .IsRequired();

            
            builder.Property(o => o.TotalAmount)
                   .IsRequired()
                   .HasPrecision(18, 2);

            builder.Property(o => o.OrderStatus)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasOne(o => o.User)
                   .WithMany(u => u.Orders)
                   .HasForeignKey(o => o.UserId)
                   .OnDelete(DeleteBehavior.NoAction);


            builder.HasOne(o => o.Payment)
                   .WithOne(p => p.Order)
                   .HasForeignKey<PaymentEntity>(p => p.OrderId)
                   ; // deleting order deletes payment

            
            builder.HasMany(o => o.OrderItems)
                   .WithOne(oi => oi.Order)
                   .HasForeignKey(oi => oi.OrderId)
                   ; // deleting order deletes all its items
        }
    }
}
