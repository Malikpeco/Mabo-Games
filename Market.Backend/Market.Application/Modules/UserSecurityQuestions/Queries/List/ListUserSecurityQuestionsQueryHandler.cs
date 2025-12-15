namespace Market.Application.Modules.UserSecurityQuestions.Queries.List
{
    public sealed class ListUserSecurityQuestionsQueryHandler : IRequestHandler<ListUserSecurityQuestionsQuery, PageResult<ListUserSecurityQuestionsQueryDto>>
    {
        public async Task<PageResult<ListUserSecurityQuestionsQueryDto>> Handle(ListUserSecurityQuestionsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
