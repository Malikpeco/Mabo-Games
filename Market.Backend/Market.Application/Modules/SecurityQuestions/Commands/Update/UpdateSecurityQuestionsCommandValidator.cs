namespace Market.Application.Modules.SecurityQuestions.Commands.Update
{
    public sealed class UpdateSecurityQuestionsCommandValidator : AbstractValidator<UpdateSecurityQuestionsCommand>
    {
        public UpdateSecurityQuestionsCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("Id must be a positive value!");
        }
    }
}
