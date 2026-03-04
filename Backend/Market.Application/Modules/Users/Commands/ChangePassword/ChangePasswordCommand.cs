using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Commands.ChangePassword
{
    public sealed class ChangePasswordCommand :IRequest<Unit>
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; } 

        public string ConfirmNewPassword { get; set; }

    }
}
