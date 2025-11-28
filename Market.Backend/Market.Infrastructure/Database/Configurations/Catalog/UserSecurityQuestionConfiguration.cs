using Market.Domain.Entities;

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

            builder.HasOne(usq => usq.User)
                   .WithMany(u => u.UserSecurityQuestions)
                   .HasForeignKey(usq => usq.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(usq => usq.SecurityQuestion)
                   .WithMany()
                   .HasForeignKey(usq => usq.SecurityQuestionId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
