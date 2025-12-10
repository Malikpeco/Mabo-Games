namespace Market.Application.Modules.SecurityQuestions.Commands.Delete
{
    public sealed class DeleteSecurityQuestionsCommandValidator : AbstractValidator<DeleteSecurityQuestionsCommand>
    {
        public DeleteSecurityQuestionsCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("Id must be a positive value!");
                
        }
    }
}
