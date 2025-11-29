namespace Market.Application.Modules.SecurityQuestions.Queries.GetById
{
    public sealed class GetSecurityQuestionsByIdQueryHandler : IRequestHandler<GetSecurityQuestionsByIdQuery, GetSecurityQuestionsByIdQueryDto>
    {
        public async Task<GetSecurityQuestionsByIdQueryDto> Handle(GetSecurityQuestionsByIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
