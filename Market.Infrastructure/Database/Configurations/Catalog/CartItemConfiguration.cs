using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                   .IsRequired()                 
                   ;


            builder.HasOne(ci => ci.Game)
                   .WithMany(g => g.CartItems)  
                   .HasForeignKey(ci => ci.GameId)
                   .IsRequired()                 
                   ; 


            builder.Property(ci => ci.AddedAt)
                   .IsRequired();

            builder.Property(ci => ci.IsSaved)
                   .IsRequired()
                   .HasDefaultValue(false);

        }

    }
}
