using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Queries.VerifyResetCode
{
    public sealed class VerifyResetCodeQueryValidator:AbstractValidator<VerifyResetCodeQuery>
    {
        public VerifyResetCodeQueryValidator() {
            RuleFor(x => x.recoveryCode)
                .NotEmpty()
                .WithMessage("Recovery code cannot be empty.");

            RuleFor(x => x.userEmail)
                .NotEmpty()
                .WithMessage("Email must not be empty");
        }

    }
}
