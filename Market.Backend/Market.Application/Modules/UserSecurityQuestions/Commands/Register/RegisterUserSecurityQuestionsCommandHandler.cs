using Market.Domain.Entities;

namespace Market.Application.Modules.UserSecurityQuestions.Commands.Create
{
    public sealed class RegisterUserSecurityQuestionsCommandHandler(IAppDbContext context, IAppCurrentUser appCurrentUser)
        : IRequestHandler<RegisterUserSecurityQuestionsCommand, int>
    {
        public async Task<int> Handle(RegisterUserSecurityQuestionsCommand request, CancellationToken ct)
        {
            var answerHasher = new PasswordHasher<UserSecurityQuestionEntity>();


            //User can only have two questions (not enforced in db, might have to change that later)
            int maxSecurityQuestionCount = await context.
                UserSecurityQuestions
                .Where(x => x.UserId == appCurrentUser.UserId)
                .CountAsync(ct);
            if (maxSecurityQuestionCount >= 2)
                throw new MarketBusinessRuleException("409", "User cannot have more than two security questions registered.");



            bool exists = await context.SecurityQuestions.AnyAsync(x => x.Id == request.SecurityQuestionId);
            if (!exists)
                throw new MarketNotFoundException("Security Question does not exist!");



            bool alreadyAdded = await context.UserSecurityQuestions.AnyAsync
                (
                x => x.UserId == appCurrentUser.UserId &&
                x.SecurityQuestionId == request.SecurityQuestionId
                );

            if (alreadyAdded)
                throw new MarketConflictException("User already has requested security question.");



            var userSecurityQuestion = new UserSecurityQuestionEntity
            {
                SecurityQuestionId = request.SecurityQuestionId,
                UserId = appCurrentUser.UserId!.Value
            };

            userSecurityQuestion.AnswerHash = answerHasher.HashPassword(userSecurityQuestion, request.SecurityQuestionAnswer);


            context.UserSecurityQuestions.Add(userSecurityQuestion);

            await context.SaveChangesAsync(ct);

            return userSecurityQuestion.Id;


        }
    }

}
