using Market.Application.Modules.UserSecurityQuestions.Queries.ListByEmail;

namespace Market.Application.Modules.UserSecurityQuestions.Queries.List
{
    public sealed record ListUserSecurityQuestionsByEmailQuery(string UserEmail) : IRequest<List<ListUserSecurityQuestionsByEmailQueryDto>>
    {
     
    }
}
