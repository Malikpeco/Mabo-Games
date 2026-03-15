

using Market.Application.Modules.SecurityQuestions.Queries.GetById;

namespace Market.Application.Modules.SecurityQuestions.Queries.List
{
    public sealed class ListSecurityQuestionsQueryHandler(IAppDbContext context,IAppCurrentUser appCurrentUser)
        : IRequestHandler<ListSecurityQuestionsQuery, PageResult<ListSecurityQuestionsQueryDto>>
    {
        public async Task<PageResult<ListSecurityQuestionsQueryDto>> Handle(ListSecurityQuestionsQuery request,CancellationToken ct)
        {
            if (!appCurrentUser.IsAdmin)
                throw new MarketForbiddenException();


            var q = context.SecurityQuestions
                .AsNoTracking();


            var projectedQ =
                q
                .OrderBy(x => x.Id)
                .Select(x => new ListSecurityQuestionsQueryDto
                {
                    Id = x.Id,
                    Question = x.Question
                });



            return await PageResult<ListSecurityQuestionsQueryDto>.FromQueryableAsync(projectedQ, request.Paging,ct);
                
        }
    }


}
