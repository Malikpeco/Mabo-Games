using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.Database.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<PaymentEntity>
    {
        public void Configure(EntityTypeBuilder<PaymentEntity> builder)
        {
            builder.ToTable("Payments");

            builder.Property(p => p.StripeTransactionId)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.PaymentStatus)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(p => p.Total)
                   .IsRequired()
                   .HasPrecision(18, 2);

            builder.Property(p => p.Date)
                   .IsRequired();

            builder.HasOne(p => p.Order)
                   .WithOne(o => o.Payment)
                   .HasForeignKey<PaymentEntity>(p => p.OrderId)
                   .IsRequired()
                   ;
            // Cascade: deleting an order deletes its payment
        }
    }
}
