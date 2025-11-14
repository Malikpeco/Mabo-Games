using Market.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Domain.Entities
{
    public class PaymentEntity : BaseEntity
    {
        public int OrderId { get; set; }
        public OrderEntity Order { get; set; }
        public string StripeTransactionId { get; set; }
        public string PaymentStatus { get; set; }
        public decimal Total { get; set; }
        public DateTime Date { get; set; }

        public PaymentEntity()
        {
            Date = DateTime.UtcNow;
            PaymentStatus = "Pending";
        }
    }
}
