using System.Xml.Schema;

namespace Market.Application.Modules.Users.Commands.Register;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{

    public RegisterUserCommandValidator()
    {

 
        RuleFor(x => x.FirstName).NotEmpty();
        
        RuleFor(x => x.LastName).NotEmpty();
        
        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(30)
            ;
        
        RuleFor(x => x.Email).EmailAddress();
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
            .WithMessage("Password does not meet minimum requirements: \n1.Must be atleast 8 characters \n2.Must contain at least one uppercase and one lowercase letter \n3.Must contain at least one number and a special character");
        
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .Equal(y => y.Password)
            .WithMessage("The passwords do not match, try again.");
        
        RuleFor(x=> x.CountryId).NotEmpty();
        
        RuleFor(x => x.PhoneNumber)
            .Matches("^\\+?(\\d{1,3})?[-.\\s]?(\\(?\\d{3}\\)?[-.\\s]?)?(\\d[-.\\s]?){6,9}\\d$")
            .WithMessage("Please enter a valid phone number. Examples: +38761234567, +385981234567.")
            .When(x=> x.PhoneNumber is not null);
            
    }
}
