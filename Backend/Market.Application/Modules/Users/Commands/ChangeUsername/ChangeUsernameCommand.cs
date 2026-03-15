using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Commands.ChangeUsername
{
    public sealed class ChangeUsernameCommand:IRequest<Unit>
    {

        public string Password { get; set; }

        public string NewUsername { get; set; }

    }
}
