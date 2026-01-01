using Market.Domain.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Commands.PasswordReset
{
    [PreserveString]
    public sealed class PasswordResetCommand:IRequest<Unit>
    {
        public string PasswordResetCode { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmNewPassword { get; set; }
    }
}
