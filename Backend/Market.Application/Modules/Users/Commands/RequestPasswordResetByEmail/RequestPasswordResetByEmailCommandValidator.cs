using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Commands.ResetPassword
{
    public sealed class RequestPasswordResetByEmailCommandValidator : AbstractValidator<RequestPasswordResetByEmailCommand>
    {
        public RequestPasswordResetByEmailCommandValidator()
        {
            RuleFor(x => x.UserEmail).EmailAddress();
        }
    }
}
