using Market.Application.Common.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Commands.PasswordReset
{
    public sealed class PasswordResetCommandHandler(IAppDbContext context, IPasswordHasher<UserEntity> passwordHasher) 
        : IRequestHandler<PasswordResetCommand, Unit>
    {
        public async Task<Unit> Handle(PasswordResetCommand request, CancellationToken cancellationToken)
        {

            DateTime currentTime = DateTime.UtcNow;


            string hashedRequestToken = SimpleHasher.Hash(request.RecoveryCode);


            var tokenEntity = await context.PasswordResetTokens
                        .FirstOrDefaultAsync(x =>
                        x.TokenHash == hashedRequestToken &&
                        !x.IsUsed &&
                        !x.IsDeleted &&
                        x.ExpiresAt > currentTime,
                        cancellationToken);

            if (tokenEntity is null)
                throw new MarketNotFoundException("Entered reset code is not valid or has expired.");


            var user = await context.Users
                .FirstOrDefaultAsync(x => x.Id == tokenEntity.UserId);

            if(user is null)
                throw new MarketNotFoundException("User not found.");

            user.PasswordHash = passwordHasher.HashPassword(user, request.NewPassword);
            tokenEntity.IsUsed = true;

            await context.SaveChangesAsync(cancellationToken);


            return Unit.Value;

        }
    }
}




