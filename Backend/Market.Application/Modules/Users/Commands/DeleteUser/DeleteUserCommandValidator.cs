namespace Market.Application.Modules.Users.Commands.DeleteUser;

public sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.ConfirmationText)
            .NotEmpty()
            .Equal("DELETE MY ACCOUNT")
            .WithMessage("Please type DELETE MY ACCOUNT to confirm account deletion.");
    }
}
