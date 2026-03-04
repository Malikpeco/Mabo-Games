using Market.Application.Modules.Users.Dto;
using Market.Domain.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Commands.RequestPasswordResetBySecurityQuestion
{
    [PreserveString]
    public sealed class RequestPasswordResetBySecurityQuestionCommand:IRequest<PasswordResetCodeDto>
    {
         public int UserSecurityQuestionId { get; set; }
         public string SecurityQuestionAnswer { get; set; }



    }
}
