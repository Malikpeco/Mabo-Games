using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Queries.CheckRecoveryOptions
{
    internal class CheckRecoveryOptionsQueryValidator:AbstractValidator<CheckRecoveryOptionsQuery>
    {
        public CheckRecoveryOptionsQueryValidator() {
        RuleFor(x=>x.RecoveryEmail).NotEmpty();
        }
    }
}
