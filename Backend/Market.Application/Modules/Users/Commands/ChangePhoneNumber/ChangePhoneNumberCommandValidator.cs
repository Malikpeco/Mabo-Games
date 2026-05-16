namespace Market.Application.Modules.Users.Commands.ChangePhoneNumber;

public sealed class ChangePhoneNumberCommandValidator : AbstractValidator<ChangePhoneNumberCommand>
{
    public ChangePhoneNumberCommandValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches("^\\+?(\\d{1,3})?[-.\\s]?(\\(?\\d{3}\\)?[-.\\s]?)?(\\d[-.\\s]?){6,9}\\d$")
            .WithMessage("Please enter a valid phone number. Examples: +38761234567, +385981234567.");
    }
}
