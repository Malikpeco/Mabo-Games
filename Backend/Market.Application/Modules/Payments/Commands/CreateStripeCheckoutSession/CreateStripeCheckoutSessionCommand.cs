using Market.Application.Modules.Payments.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Payments.Commands.CreateStripeCheckoutSession
{
    public sealed record CreateStripeCheckoutSessionCommand(int OrderId)
        : IRequest<CreateStripeCheckoutSessionResponse>;
}
