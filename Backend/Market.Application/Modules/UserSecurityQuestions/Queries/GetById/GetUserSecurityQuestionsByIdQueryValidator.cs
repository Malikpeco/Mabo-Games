namespace Market.Application.Modules.UserSecurityQuestions.Queries.GetById
{
    public sealed class GetUserSecurityQuestionsByIdQueryValidator : AbstractValidator<GetUserSecurityQuestionsByIdQuery>
    {
        public GetUserSecurityQuestionsByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
