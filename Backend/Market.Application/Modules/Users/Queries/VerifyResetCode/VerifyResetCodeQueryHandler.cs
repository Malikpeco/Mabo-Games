using Market.Application.Common.Security;
using Market.Application.Modules.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Queries.VerifyResetCode
{
    public sealed class VerifyResetCodeQueryHandler(IAppDbContext context) : IRequestHandler<VerifyResetCodeQuery, Unit>
    {
        public async Task<Unit> Handle(VerifyResetCodeQuery request, CancellationToken cancellationToken)
        {
            var hashedRequestToken = SimpleHasher.Hash(request.recoveryCode);
            var currentTime = DateTime.UtcNow;

            var requestUser = await context.Users.FirstOrDefaultAsync(x => x.Email == request.userEmail);

            if (requestUser == null)
                throw new MarketNotFoundException("Invalid user request.");

            var tokenEntity = await context.PasswordResetTokens
                        .FirstOrDefaultAsync(x =>
                        x.TokenHash == hashedRequestToken &&
                        !x.IsUsed &&
                        x.UserId == requestUser.Id &&
                        !x.IsDeleted &&
                        x.ExpiresAt > currentTime,
                        cancellationToken);

            if (tokenEntity is null)
                throw new MarketNotFoundException("Entered reset code is not valid or has expired.");


            return Unit.Value;
            
        }
    }
}
