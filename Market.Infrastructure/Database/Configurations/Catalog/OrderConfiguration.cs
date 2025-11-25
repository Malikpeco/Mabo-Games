using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                   .IsRequired()
                   ; // can't delete user if they have orders

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
