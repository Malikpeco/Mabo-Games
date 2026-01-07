using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Commands.ChangeBio
{
    public sealed class ChangeProfileBioCommand:IRequest<Unit>
    {
        public string? NewBio {  get; set; }

    }
}
