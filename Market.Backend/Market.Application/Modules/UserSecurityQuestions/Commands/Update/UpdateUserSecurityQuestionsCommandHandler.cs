using Market.Application.Modules.SecurityQuestions.Queries.GetById;
using Market.Domain.Entities;
using System.Security.Cryptography.X509Certificates;

namespace Market.Application.Modules.UserSecurityQuestions.Commands.Update
{
    public sealed class UpdateUserSecurityQuestionsCommandHandler(IAppDbContext context,IAppCurrentUser appCurrentUser)
        : IRequestHandler<UpdateUserSecurityQuestionsCommand, int>
    {
        public async Task<int> Handle(UpdateUserSecurityQuestionsCommand request, CancellationToken cancellationToken)
        {
            var answerHasher = new PasswordHasher<UserSecurityQuestionEntity>();

            var userQuestion = await context.UserSecurityQuestions
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == appCurrentUser.UserId, cancellationToken);

            if (userQuestion == null)
                throw new MarketNotFoundException("Invalid user question");


             userQuestion.AnswerHash= answerHasher.HashPassword(userQuestion, request.newAnswer);

           
            await context.SaveChangesAsync(cancellationToken);

            return userQuestion.Id;


        }
    }

}
