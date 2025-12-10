namespace Market.Application.Modules.SecurityQuestions.Commands.Create
{
    public sealed class CreateSecurityQuestionsCommandValidator : AbstractValidator<CreateSecurityQuestionsCommand>
    {
        public CreateSecurityQuestionsCommandValidator()
        {
            RuleFor(x => x.Question)
               .NotEmpty()
               .MinimumLength(10);
        }
    }
}
