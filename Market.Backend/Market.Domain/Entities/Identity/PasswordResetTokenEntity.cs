using Market.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Domain.Entities.Identity
{
    public class PasswordResetTokenEntity :BaseEntity
    {

        public int Id { get; set; }

        public int UserId { get; set; }

        public UserEntity User { get; set; }

        public string TokenHash { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool IsUsed { get; set; }

    }
}
