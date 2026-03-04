using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Commands.ChangeUsername
{
    public sealed class ChangeUsernameCommandValidator : AbstractValidator<ChangeUsernameCommand>
    {
        public ChangeUsernameCommandValidator()
        {

            RuleFor(x => x.NewUsername)
                .MinimumLength(5)
                .MaximumLength(30);

            RuleFor(x => x.Password)
                 .NotEmpty()
                 .WithMessage("Password cannot be empty");
                    


        }

    }
}
