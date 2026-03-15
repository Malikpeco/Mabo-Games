using Market.Application.Abstractions;

namespace Market.Application.Modules.SecurityQuestions.Queries.GetById
{
    public sealed class GetSecurityQuestionsByIdQueryHandler (IAppDbContext context,IAppCurrentUser appCurrentUser) 
        : IRequestHandler<GetSecurityQuestionsByIdQuery, GetSecurityQuestionsByIdQueryDto>
    {
        public async Task<GetSecurityQuestionsByIdQueryDto> Handle(GetSecurityQuestionsByIdQuery request, CancellationToken ct)
        {
            if (!appCurrentUser.IsAdmin)
                throw new MarketForbiddenException();

            var q = await context.SecurityQuestions
                .AsNoTracking()
                .Where(x => x.Id == request.Id)
                .Select(y=>new GetSecurityQuestionsByIdQueryDto
                {
                    Id= y.Id,
                    Question=y.Question
                }
                )
                .FirstOrDefaultAsync(ct);

            if (q == null)
                throw new MarketNotFoundException($"Security question with the id: {request.Id} was not found!");

            return q;

        }
    }

}
