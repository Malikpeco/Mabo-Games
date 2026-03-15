using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Commands.RequestPasswordResetBySecurityQuestion
{
    public sealed class RequestPasswordResetBySecurityQuestionCommandValidator:AbstractValidator<RequestPasswordResetBySecurityQuestionCommand>
    {
        public RequestPasswordResetBySecurityQuestionCommandValidator()
        {
            RuleFor(x=> x.UserSecurityQuestionId)
                .GreaterThan(0).WithMessage("UserSecurityQuestionId must be a positive value.");

            RuleFor(x=>x.SecurityQuestionAnswer)
                .NotEmpty().WithMessage("SecurityQuestionAnswer is required.")
                .MaximumLength(200).WithMessage("SecurityQuestionAnswer must not exceed 200 characters.");

        }
    }
}
