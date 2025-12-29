using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Payments.Commands.ProcessStripeWebhook
{
    public sealed record ProcessStripeWebhookCommand(
        string EventId,
        string EventType,
        string? SessionId,
        string? PaymentIntentId,
        int? OrderId) 
        : IRequest<Unit>;
}
