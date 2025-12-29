using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Abstractions
{
    public sealed class StripeOptions
    {
        public string SecretKey { get; init; } = default!;
        public string WebhookSecret { get; init; } = default!;
        public string SuccessUrl { get; init; } = default!;
        public string CancelUrl { get; init; } = default!;
    }
}

