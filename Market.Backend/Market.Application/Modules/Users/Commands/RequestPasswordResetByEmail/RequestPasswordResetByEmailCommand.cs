using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Commands.ResetPassword
{
    public sealed class RequestPasswordResetByEmailCommand : IRequest<Unit>
    {
        public string UserEmail { get; set; }

    }
}
