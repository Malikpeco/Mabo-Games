namespace Market.Application.Modules.SecurityQuestions.Queries.GetById
{
    public sealed class GetSecurityQuestionsByIdQuery : IRequest<GetSecurityQuestionsByIdQueryDto>
    {
        public int Id { get; set; }
    }
}
