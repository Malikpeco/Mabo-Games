namespace Market.Application.Modules.UserSecurityQuestions.Commands.Update
{
    public sealed class UpdateUserSecurityQuestionsCommandValidator : AbstractValidator<UpdateUserSecurityQuestionsCommand>
    {
        public UpdateUserSecurityQuestionsCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0).WithMessage("Id must be a positive value!");

            RuleFor(x=> x.newAnswer)
                .NotEmpty().WithMessage("The new answer cannot be empty.")
                .MinimumLength(3)
                .MaximumLength(100).WithMessage("The new answer cannot exceed 100 characters.");
        }
    }
}
