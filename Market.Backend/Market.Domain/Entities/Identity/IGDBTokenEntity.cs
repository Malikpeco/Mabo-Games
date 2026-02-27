using Market.Domain.Common;
using Market.Domain.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Domain.Entities.Identity
{
    public sealed class IGDBTokenEntity :BaseEntity
    {

        [NoAudit]
        [PreserveString]
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }

        public DateTime LastUpdated {  get; set; }
    }
}
