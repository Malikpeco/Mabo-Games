using Market.Application.Abstractions;
using Market.Application.Modules.SecurityQuestions.Dto;

namespace Market.Application.Modules.SecurityQuestions.Commands.Update
{
    public sealed class UpdateSecurityQuestionCommandHandler(IAppDbContext context, IAppCurrentUser appCurrentUser)
        : IRequestHandler<UpdateSecurityQuestionCommand, UpdateSecurityQuestionResultDto>
    {
        public async Task<UpdateSecurityQuestionResultDto> Handle(UpdateSecurityQuestionCommand request, CancellationToken cancellationToken)
        {
            if (!appCurrentUser.IsAdmin)
                throw new MarketForbiddenException();

            var securityQuestion = await context.SecurityQuestions
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (securityQuestion is null)
                throw new MarketNotFoundException("Security Question does not exist.");

           
            //For OldQuestion
            string oldSecurityQuestion=securityQuestion.Question;

            securityQuestion.Question=request.NewQuestion;


            await context.SaveChangesAsync(cancellationToken);

            return new UpdateSecurityQuestionResultDto()
            {
                Id = securityQuestion.Id,
                NewQuestion = securityQuestion.Question,
                OldQuestion = oldSecurityQuestion

            };

        }
    }

}
