using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.Database.Configurations
{
    public class UserSecurityQuestionConfiguration : IEntityTypeConfiguration<UserSecurityQuestionEntity>
    {
        public void Configure(EntityTypeBuilder<UserSecurityQuestionEntity> builder)
        {
            builder.ToTable("UserSecurityQuestions");

            builder.Property(uq => uq.AnswerHash)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.HasOne(uq => uq.User)
                   .WithMany(u => u.UserSecurityQuestions)
                   .HasForeignKey(uq => uq.UserId)
                   ;

            builder.HasOne(uq => uq.SecurityQuestion)
                   .WithMany(sq => sq.UserSecurityQuestions)
                   .HasForeignKey(uq => uq.SecurityQuestionId)
                   ;
        }
    }
}
