using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Abstractions
{
    public  interface IEmailSender
    {
        Task SendPasswordRecoveryCode(string recieverEmail, string recoverCode, CancellationToken cancellationTokens);
    }
    
}
