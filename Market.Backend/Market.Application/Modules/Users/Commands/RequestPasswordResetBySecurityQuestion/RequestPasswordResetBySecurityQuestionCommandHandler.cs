using Market.Application.Abstractions;
using Market.Application.Common.Security;
using Market.Application.Modules.Users.Commands.RequestPasswordResetBySecurityQuestion;
using Market.Application.Modules.Users.Dto;
using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Commands.RequestPasswordResetBySecurityQuestion
{
    public sealed class RequestPasswordResetBySecurityQuestionCommandHandler(
        IAppDbContext context, 
        IPasswordHasher<UserSecurityQuestionEntity> answerHasher)
        : IRequestHandler<RequestPasswordResetBySecurityQuestionCommand, PasswordResetCodeDto>
    {
        public async Task<PasswordResetCodeDto> Handle(RequestPasswordResetBySecurityQuestionCommand request, CancellationToken cancellationToken)
        {

            var userSecurityQuestion = await context.UserSecurityQuestions
                .FirstOrDefaultAsync(x=> x.Id == request.UserSecurityQuestionId, cancellationToken);

            if (userSecurityQuestion == null)
                throw new MarketNotFoundException("User security question not found.");

            var verify = answerHasher.VerifyHashedPassword(userSecurityQuestion, userSecurityQuestion.AnswerHash, request.SecurityQuestionAnswer);
            if (verify == PasswordVerificationResult.Failed)
                throw new MarketConflictException("Incorrect security question answer.");


            var randomCode = RandomNumberGenerator.GetInt32(100000, 999999).ToString();


            var passwordResetToken = new PasswordResetTokenEntity()
            {
                UserId = userSecurityQuestion.UserId,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                IsUsed = false,
            };

            passwordResetToken.TokenHash = SimpleHasher.Hash(randomCode);

            context.PasswordResetTokens.Add(passwordResetToken);

            await context.SaveChangesAsync(cancellationToken);

            return new PasswordResetCodeDto() { ResetCode=randomCode};
        }
    }
}
