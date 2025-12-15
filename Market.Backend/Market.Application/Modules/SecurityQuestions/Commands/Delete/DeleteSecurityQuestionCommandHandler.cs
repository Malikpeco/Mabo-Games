using Market.Application.Modules.SecurityQuestions.Dto;

namespace Market.Application.Modules.SecurityQuestions.Commands.Delete
{
    public sealed class DeleteSecurityQuestionCommandHandler(IAppDbContext context, IAppCurrentUser appCurrentUser) 
        : IRequestHandler<DeleteSecurityQuestionCommand, DeleteSecurityQuestionResultDto>
    {
        public async Task<DeleteSecurityQuestionResultDto> Handle(DeleteSecurityQuestionCommand request, CancellationToken cancellationToken)
        {
            if (!appCurrentUser.IsAdmin)
                throw new MarketBusinessRuleException("123", "Unauthorized access, you do not have admin privileges.");

            var securityQuestion = await context.SecurityQuestions
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (securityQuestion is null)
                throw new MarketNotFoundException("Security Question does not exist.");

            securityQuestion.IsDeleted = true; // Soft delete
            await context.SaveChangesAsync(cancellationToken);

            return new DeleteSecurityQuestionResultDto()
            {
                Id = securityQuestion.Id,
                Question = securityQuestion.Question
            };


        }
    }

}
