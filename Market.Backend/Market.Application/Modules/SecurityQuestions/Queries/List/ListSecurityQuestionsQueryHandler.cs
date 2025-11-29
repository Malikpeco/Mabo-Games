namespace Market.Application.Modules.SecurityQuestions.Queries.List
{
    public sealed class ListSecurityQuestionsQueryHandler : IRequestHandler<ListSecurityQuestionsQuery, PageResult<ListSecurityQuestionsQueryDto>>
    {
        public async Task<PageResult<ListSecurityQuestionsQueryDto>> Handle(ListSecurityQuestionsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
