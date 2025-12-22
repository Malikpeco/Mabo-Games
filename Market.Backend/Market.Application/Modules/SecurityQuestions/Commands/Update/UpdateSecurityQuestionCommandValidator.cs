namespace Market.Application.Modules.SecurityQuestions.Commands.Update
{
    public sealed class UpdateSecurityQuestionCommandValidator : AbstractValidator<UpdateSecurityQuestionCommand>
    {
        public UpdateSecurityQuestionCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("Id must be a positive value!");
        }
    }
}
