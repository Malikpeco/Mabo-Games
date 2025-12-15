using Market.Application.Abstractions;
using Market.Application.Modules.SecurityQuestions.Dto;
using Market.Domain.Entities;

namespace Market.Application.Modules.SecurityQuestions.Commands.Create
{
    public sealed class CreateSecurityQuestionCommandHandler(IAppDbContext context, IAppCurrentUser appCurrentUser) 
        : IRequestHandler<CreateSecurityQuestionCommand, CreateSecurityQuestionResultDto>
        
    {
        public async Task<CreateSecurityQuestionResultDto> Handle(CreateSecurityQuestionCommand request, CancellationToken cancellationToken)
        {
            //Only admins are allow to add security questions
            if (!appCurrentUser.IsAdmin)
                throw new MarketBusinessRuleException("123", "Unauthorized access, you do not have admin privileges.");

            bool exists = await context.SecurityQuestions.AnyAsync(x => x.Question == request.Question);

            if (exists) { throw new MarketConflictException("Security Question already exists."); }

            var securityQuestion = new SecurityQuestionEntity()
            {
                Question = request.Question!.Trim(),
            };

            context.SecurityQuestions.Add(securityQuestion);
            
            await context.SaveChangesAsync(cancellationToken);

            return new CreateSecurityQuestionResultDto()
            {
                Id = securityQuestion.Id,
                Question = securityQuestion.Question,
            };

        }
    }

}
