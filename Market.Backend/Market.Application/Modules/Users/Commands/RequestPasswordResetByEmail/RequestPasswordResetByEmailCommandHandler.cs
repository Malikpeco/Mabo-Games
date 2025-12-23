using Market.Application.Common.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Commands.ResetPassword
{
    public sealed class RequestPasswordResetByEmailCommandHandler(IAppDbContext context,IEmailSender emailSender)
        : IRequestHandler<RequestPasswordResetByEmailCommand, Unit>
        
        
    {
        public async Task<Unit> Handle(RequestPasswordResetByEmailCommand request,CancellationToken cancellationToken)
        {


            var user= await context.Users.FirstOrDefaultAsync(x=>x.Email==request.UserEmail,cancellationToken);

            if (user is null)
                return Unit.Value;


            var randomCode = RandomNumberGenerator.GetInt32(100000, 999999).ToString();


            var PasswordResetToken = new PasswordResetTokenEntity()
            {
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                IsUsed = false,
            };

            PasswordResetToken.TokenHash=SimpleHasher.Hash(randomCode);

            context.PasswordResetTokens.Add(PasswordResetToken);

            await context.SaveChangesAsync(cancellationToken);

            await emailSender.SendPasswordRecoveryCode(user.Email, randomCode, cancellationToken);
            
            return Unit.Value;
        }
    }
}
