namespace Market.Application.Modules.Users.Commands.ChangePhoneNumber;

public sealed class ChangePhoneNumberCommand : IRequest<Unit>
{
    public string PhoneNumber { get; set; }
}
