namespace Market.Application.Modules.UserSecurityQuestions.Queries.GetById
{
    public sealed class GetUserSecurityQuestionsByIdQuery : IRequest<GetUserSecurityQuestionsByIdQueryDto>
    {
        public int Id { get; set; }
    }
}
