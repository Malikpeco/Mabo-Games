namespace Market.Application.Modules.SecurityQuestions.Commands.Delete
{
    public sealed class DeleteSecurityQuestionCommandValidator : AbstractValidator<DeleteSecurityQuestionCommand>
    {
        public DeleteSecurityQuestionCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("Id must be a positive value!");
                
        }
    }
}
