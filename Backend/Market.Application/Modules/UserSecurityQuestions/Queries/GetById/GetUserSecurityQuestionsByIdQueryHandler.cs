using Market.Application.Modules.SecurityQuestions.Queries.GetById;
using System.ComponentModel.DataAnnotations;

namespace Market.Application.Modules.UserSecurityQuestions.Queries.GetById
{
    public sealed class GetUserSecurityQuestionsByIdQueryHandler(IAppDbContext context, IAppCurrentUser appCurrentUser)
        : IRequestHandler<GetUserSecurityQuestionsByIdQuery, GetUserSecurityQuestionsByIdQueryDto>
    {
        public async Task<GetUserSecurityQuestionsByIdQueryDto> Handle(GetUserSecurityQuestionsByIdQuery request, CancellationToken ct)
        {

            bool exists = await context.SecurityQuestions.AnyAsync(x => x.Id == request.Id);


            var q = await context.UserSecurityQuestions
               .AsNoTracking()
               .Where(
                x => x.Id == request.Id &&
                x.UserId == appCurrentUser.UserId
               )
               .Select(y => new GetUserSecurityQuestionsByIdQueryDto
               {
                   Id = y.Id,
                   Question = y.SecurityQuestion.Question
               }
               )
               .FirstOrDefaultAsync(ct);

            if (q == null)
                throw new MarketNotFoundException($"Security Question with the id: {request.Id} was not found for the current user.");

            return q;





        }
    }

}
