using Market.Domain.Common;
using Market.Domain.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Domain.Entities.Identity
{
    public class PasswordResetTokenEntity :BaseEntity
    {

        public int UserId { get; set; }

        public UserEntity User { get; set; }

        [NoAudit]
        public string TokenHash { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool IsUsed { get; set; }

    }
}
