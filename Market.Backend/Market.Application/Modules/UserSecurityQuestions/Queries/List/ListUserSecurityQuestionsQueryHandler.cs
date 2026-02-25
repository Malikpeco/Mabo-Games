namespace Market.Application.Modules.UserSecurityQuestions.Queries.List
{
    public sealed class ListUserSecurityQuestionsQueryHandler(IAppDbContext context, IAppCurrentUser appCurrentUser)
        : IRequestHandler<ListUserSecurityQuestionsQuery,List<ListUserSecurityQuestionsQueryDto>>
    {
        public async Task<List<ListUserSecurityQuestionsQueryDto>> Handle(ListUserSecurityQuestionsQuery request, CancellationToken ct)
        {

            var q = context.UserSecurityQuestions.
                AsNoTracking();
               

            return await 
                q.
                Where(x=>x.UserId==appCurrentUser.UserId).
                OrderBy(x=>x.Id).
                Select(x=> new ListUserSecurityQuestionsQueryDto
                {
                    Id = x.Id,
                    Question=x.SecurityQuestion.Question,
                }
                ).
                ToListAsync(ct);

        }
    }

}
