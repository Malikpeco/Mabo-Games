using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Queries.VerifyResetCode
{
    public sealed record VerifyResetCodeQuery(string recoveryCode, string userEmail):IRequest<Unit>
    {

    }
}
