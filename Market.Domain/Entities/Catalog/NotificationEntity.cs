using Market.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Domain.Entities
{
    public class NotificationEntity : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentAt { get; set; }
        public int UserId { get; set; }
        public UserEntity User { get; set; }

        public NotificationEntity()
        {
            IsRead = false;            // unread by default
            SentAt = DateTime.UtcNow;  // timestamp when created
        }
    }
}
