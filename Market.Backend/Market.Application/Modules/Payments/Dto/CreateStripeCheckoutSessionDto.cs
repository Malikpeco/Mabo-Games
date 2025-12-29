using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Payments.Dto
{
    public sealed record CreateStripeCheckoutSessionRequest(int OrderId);
    //what client sends to api endpoint, we only need OrderId to start a stripe checkout for a specific order


    public sealed record CreateStripeCheckoutSessionResponse(
        int OrderId,
        string SessionId,
        string CheckoutUrl,
        DateTime ExpiresAtUtc);
}
