using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.Database.Configurations.Identity
{
    public sealed class PasswordResetTokenEntityConfiguration : IEntityTypeConfiguration<PasswordResetTokenEntity>
    {
        public void Configure(EntityTypeBuilder<PasswordResetTokenEntity> b)
        {
            b.ToTable("PasswordResetTokens");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.TokenHash)
                .IsUnique();
            b.Property(x => x.TokenHash)
                .IsRequired()
                .HasMaxLength(256);
            b.Property(x => x.ExpiresAt)
                .IsRequired();
            b.Property(x => x.IsUsed)
                .HasDefaultValue(false);

            b.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        }

    }




}

