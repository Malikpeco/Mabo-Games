using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Commands.ChangePassword
{
    public sealed class ChangePasswordCommandHandler(IAppCurrentUser appCurrentUser, IAppDbContext context,
        IEmailSender emailSender, IPasswordHasher<UserEntity> passwordHasher) : IRequestHandler<ChangePasswordCommand, Unit>
    {
        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (!appCurrentUser.IsAuthenticated)
                throw new MarketForbiddenException();

            var user = await context.Users.FirstAsync(x => x.Id == appCurrentUser.UserId, cancellationToken);


            var passwordsMatch = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.OldPassword);

            if (passwordsMatch == PasswordVerificationResult.Failed)
                throw new MarketForbiddenException(403, "Entered password is incorrect, try again.");


            user.PasswordHash = passwordHasher.HashPassword(user, request.NewPassword);

            await context.SaveChangesAsync(cancellationToken);


            await emailSender.SendEmail(user.Email, "Password Change",
                $"Hi {user.FirstName},\n\nYour password has been successfully changed"+
                $"\n\nIf you did not request a password change, please contact our support team.", cancellationToken);


            return Unit.Value;


        }
    }
}
