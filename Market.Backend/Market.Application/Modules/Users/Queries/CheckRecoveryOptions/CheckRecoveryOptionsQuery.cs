using Market.Application.Modules.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Queries.CheckRecoveryOptions
{
    public sealed record CheckRecoveryOptionsQuery(string RecoveryEmail):IRequest<CheckRecoveryOptionsDto>
    {
    }
}
