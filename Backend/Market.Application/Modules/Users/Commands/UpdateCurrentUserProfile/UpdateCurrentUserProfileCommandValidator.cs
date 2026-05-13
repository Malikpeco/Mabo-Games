namespace Market.Application.Modules.Users.Commands.UpdateCurrentUserProfile
{
    public sealed class UpdateCurrentUserProfileCommandValidator : AbstractValidator<UpdateCurrentUserProfileCommand>
    {
        public UpdateCurrentUserProfileCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(50);

            RuleFor(x => x.Bio)
                .MaximumLength(500);
        }
    }
}