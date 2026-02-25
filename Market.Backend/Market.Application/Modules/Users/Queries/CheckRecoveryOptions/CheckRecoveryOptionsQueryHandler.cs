using Market.Application.Modules.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Queries.CheckRecoveryOptions
{
    public class CheckRecoveryOptionsQueryHandler(IAppDbContext context) 
        : IRequestHandler<CheckRecoveryOptionsQuery, CheckRecoveryOptionsDto>
    {
        public async Task<CheckRecoveryOptionsDto> Handle(CheckRecoveryOptionsQuery request, CancellationToken cancellationToken)
        {

            bool exists = await context.Users.
                AsNoTracking().
                AnyAsync(x=>x.Email==request.RecoveryEmail,cancellationToken);

            if (!exists)
                throw new MarketNotFoundException("User with that email was not found");

            bool hasSecurityQuestions= await context.Users.
                AsNoTracking().
                AnyAsync(x=>x.Email == request.RecoveryEmail 
            && x.UserSecurityQuestions.Any(),cancellationToken);

            return hasSecurityQuestions ? 
                new CheckRecoveryOptionsDto() { CanUseSecurityQuestionRecovery=true} : new CheckRecoveryOptionsDto();

        }
    }
}
