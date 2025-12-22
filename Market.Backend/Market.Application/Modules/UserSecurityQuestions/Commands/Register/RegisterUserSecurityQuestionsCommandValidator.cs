namespace Market.Application.Modules.UserSecurityQuestions.Commands.Create
{
    public sealed class RegisterUserSecurityQuestionsCommandValidator : AbstractValidator<RegisterUserSecurityQuestionsCommand>
    {
        public RegisterUserSecurityQuestionsCommandValidator()
        {
            RuleFor(x => x.SecurityQuestionId)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.SecurityQuestionAnswer)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(100);
        }
    }
}
