using System.Security.Cryptography.X509Certificates;

namespace Market.Application.Modules.UserSecurityQuestions.Queries.ListByEmail
{
    public sealed class ListUserSecurityQuestionsByEmailQueryValidator : AbstractValidator<ListUserSecurityQuestionsByEmailQuery>
    {
        public ListUserSecurityQuestionsByEmailQueryValidator()
        {
            RuleFor(x => x.userEmail).NotEmpty();
        }
    }
}
