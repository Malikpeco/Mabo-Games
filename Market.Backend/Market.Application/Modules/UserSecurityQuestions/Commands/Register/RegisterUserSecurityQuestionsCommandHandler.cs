using Market.Domain.Entities;

namespace Market.Application.Modules.UserSecurityQuestions.Commands.Create
{
    public sealed class RegisterUserSecurityQuestionsCommandHandler(IAppDbContext context, IAppCurrentUser appCurrentUser)
        : IRequestHandler<RegisterUserSecurityQuestionsCommand, int>
    {
        public async Task<int> Handle(RegisterUserSecurityQuestionsCommand request, CancellationToken cancellationToken)
        {
            var answerHasher = new PasswordHasher<UserSecurityQuestionEntity>();


            bool exists = await context.SecurityQuestions.AnyAsync(x => x.Id == request.SecurityQuestionId);


            if (!exists)
                throw new MarketNotFoundException("Security Question does not exist!");



            bool alreadyAdded = await context.UserSecurityQuestions.AnyAsync
                (
                x => x.UserId == appCurrentUser.UserId!.Value &&
                x.SecurityQuestionId == request.SecurityQuestionId
                );

            if (alreadyAdded)
                throw new MarketConflictException("User already has requested security question!");
               
        




            var userSecurityQuestion = new UserSecurityQuestionEntity
            {
                SecurityQuestionId = request.SecurityQuestionId,
                UserId = appCurrentUser.UserId!.Value
            };

            userSecurityQuestion.AnswerHash = answerHasher.HashPassword(userSecurityQuestion, request.SecurityQuestionAnswer);


            context.UserSecurityQuestions.Add(userSecurityQuestion);

            await context.SaveChangesAsync(cancellationToken);

            return userSecurityQuestion.Id;


        }
    }

}
