namespace Market.Application.Modules.SecurityQuestions.Queries.GetById
{
    public sealed class GetSecurityQuestionsByIdQueryValidator : AbstractValidator<GetSecurityQuestionsByIdQuery>
    {
        public GetSecurityQuestionsByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id must be a positive value!");
                
        }
    }
}
