namespace Market.Application.Modules.SecurityQuestions.Commands.Create
{
    public sealed class CreateSecurityQuestionCommandValidator : AbstractValidator<CreateSecurityQuestionCommand>
    {
        public CreateSecurityQuestionCommandValidator()
        {
            RuleFor(x => x.Question)
               .NotEmpty()
               .MinimumLength(10);
        }
    }
}
