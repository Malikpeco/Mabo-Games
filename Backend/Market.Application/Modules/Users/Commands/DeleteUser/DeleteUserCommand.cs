namespace Market.Application.Modules.Users.Commands.DeleteUser;

public sealed class DeleteUserCommand : IRequest<Unit>
{
    public string ConfirmationText { get; set; }
}
