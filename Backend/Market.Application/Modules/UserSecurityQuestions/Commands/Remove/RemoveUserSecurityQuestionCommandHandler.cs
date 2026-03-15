using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.UserSecurityQuestions.Commands.Remove
{
    public sealed class RemoveUserSecurityQuestionCommandHandler(IAppCurrentUser appCurrentUser, IAppDbContext context)
        : IRequestHandler<RemoveUserSecurityQuestionCommand, int>
    {
        public async Task<int> Handle(RemoveUserSecurityQuestionCommand request, CancellationToken cancellationToken)
        {

            bool exist =  await context.UserSecurityQuestions
                .AnyAsync(x=>x.UserId == appCurrentUser.UserId);

            if (!exist)
                throw new MarketConflictException("Invalid security question operation");

            var userSecurityQuestion= await context.UserSecurityQuestions
                .FirstOrDefaultAsync(x=>x.SecurityQuestionId == request.Id && x.UserId == appCurrentUser.UserId,cancellationToken) ;

            if (userSecurityQuestion == null)
                throw new MarketNotFoundException("Invalid user question ");


            userSecurityQuestion.IsDeleted= true ;

            await context.SaveChangesAsync(cancellationToken);


            return userSecurityQuestion.Id;
        }
    }
}
