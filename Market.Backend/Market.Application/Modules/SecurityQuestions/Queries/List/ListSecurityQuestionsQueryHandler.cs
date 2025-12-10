

using Market.Application.Modules.SecurityQuestions.Queries.GetById;

namespace Market.Application.Modules.SecurityQuestions.Queries.List
{
    public sealed class ListSecurityQuestionsQueryHandler(IAppDbContext context)
        : IRequestHandler<ListSecurityQuestionsQuery, List<ListSecurityQuestionsQueryDto>>
    {
        public async Task<List<ListSecurityQuestionsQueryDto>> Handle(
            ListSecurityQuestionsQuery request,
            CancellationToken ct)
        {
            var q = context.SecurityQuestions
                .AsNoTracking();

            return await q
                .OrderBy(x => x.Id)
                .Select(x => new ListSecurityQuestionsQueryDto
                {
                    Id = x.Id,
                    Question = x.Question
                })
                .ToListAsync(ct);
        }
    }


}
