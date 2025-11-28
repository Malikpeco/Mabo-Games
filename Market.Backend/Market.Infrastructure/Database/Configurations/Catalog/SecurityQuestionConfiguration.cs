using Market.Domain.Entities;

namespace Market.Infrastructure.Database.Configurations
{
    public class SecurityQuestionConfiguration : IEntityTypeConfiguration<SecurityQuestionEntity>
    {
        public void Configure(EntityTypeBuilder<SecurityQuestionEntity> builder)
        {
            builder.ToTable("SecurityQuestions");

            builder.Property(sq => sq.Question)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasMany(sq => sq.UserSecurityQuestions)
                   .WithOne(usq => usq.SecurityQuestion)
                   .HasForeignKey(usq => usq.SecurityQuestionId)
                   .IsRequired()
                   ;
            // Cascade: deleting a security question removes all related UserSecurityQuestion links
        }
    }
}
