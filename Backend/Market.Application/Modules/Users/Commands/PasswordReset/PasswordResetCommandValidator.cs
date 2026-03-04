using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Commands.PasswordReset
{
    public sealed class PasswordResetCommandValidator:AbstractValidator <PasswordResetCommand>
    {
        public PasswordResetCommandValidator()
        {
            RuleFor(x => x.NewPassword).NotEmpty()
               .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
               .WithMessage("Password does not meet minimum requirements: \n1.Must be atleast 8 characters \n2.Must contain at least one uppercase and one lowercase letter \n3.Must contain at least one number and a special character");

            RuleFor(x => x.ConfirmNewPassword).Equal(y => y.NewPassword)
            .WithMessage("The passwords do not match, try again.");
        }
    }
}
