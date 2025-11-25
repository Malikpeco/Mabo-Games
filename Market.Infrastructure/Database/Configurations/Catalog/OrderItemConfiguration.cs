using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            builder.HasOne(oi => oi.Game)
                   .WithMany(g => g.OrderItems)
                   .HasForeignKey(oi => oi.GameId)
                   .IsRequired()
                   ;
            // Restrict: can't delete a game if it exists in any order item


            builder.HasOne(oi => oi.Order)
                   .WithMany(o => o.OrderItems)
                   .HasForeignKey(oi => oi.OrderId)
                   .IsRequired()
                   ;
            // Cascade: deleting an order deletes its items
        }
    }
}
