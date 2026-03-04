using Market.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Domain.Entities.Catalog
{
    public class ProcessedWebhookEventEntity : BaseEntity
    {
        public string Provider { get; set; } = "Stripe";
        public string EventId { get; set; }
        public string EventType { get; set; }
        public DateTime ReceivedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
