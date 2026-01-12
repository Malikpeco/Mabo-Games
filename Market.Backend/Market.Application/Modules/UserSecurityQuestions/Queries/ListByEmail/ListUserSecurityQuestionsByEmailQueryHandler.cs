namespace Market.Application.Modules.UserSecurityQuestions.Queries.ListByEmail
{
    public sealed class ListUserSecurityQuestionsByEmailQueryHandler(IAppDbContext context, IAppCurrentUser appCurrentUser)
        : IRequestHandler<ListUserSecurityQuestionsByEmailQuery,List<ListUserSecurityQuestionsByEmailQueryDto>>
    {
        public async Task<List<ListUserSecurityQuestionsByEmailQueryDto>> Handle(ListUserSecurityQuestionsByEmailQuery request, CancellationToken ct)
        {

            var userQ = await context.Users.AsNoTracking()
                .FirstAsync(x => x.Email == request.userEmail,ct);

            if (userQ == null)
                throw new MarketNotFoundException("User was not found");


            var questionsQ = context.UserSecurityQuestions.
                AsNoTracking();
        

            return await 
                questionsQ.AsNoTracking().
                Where(x=>x.UserId==userQ.Id).
                OrderBy(x=>x.Id).
                Select(x=> new ListUserSecurityQuestionsByEmailQueryDto 
                {
                    Id = x.Id,
                    Question=x.SecurityQuestion.Question,
                }
                ).
                ToListAsync(ct);

        }
    }

}
