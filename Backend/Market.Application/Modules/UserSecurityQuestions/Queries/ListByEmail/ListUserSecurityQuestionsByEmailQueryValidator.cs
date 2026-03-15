using Market.Application.Modules.UserSecurityQuestions.Queries.List;
using System.Security.Cryptography.X509Certificates;

namespace Market.Application.Modules.UserSecurityQuestions.Queries.ListByEmail
{
    public sealed class ListUserSecurityQuestionsByEmailQueryValidator : AbstractValidator<ListUserSecurityQuestionsByEmailQuery>
    {
        public ListUserSecurityQuestionsByEmailQueryValidator()
        {
            RuleFor(x => x.UserEmail).NotEmpty();
        }
    }
}
