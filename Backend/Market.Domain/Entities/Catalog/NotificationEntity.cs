using Market.Domain.Common;
using Market.Domain.Common.Attributes;
using Market.Domain.Entities.Catalog;
using Market.Domain.Entities.Identity;

namespace Market.Domain.Entities
{
    [NoAudit]
    [PreserveString]
    public class NotificationEntity : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }

        public IReadOnlyCollection<UserNotificationsEntity> UserNotifications { get; private set; } = new List<UserNotificationsEntity>();


        public NotificationEntity()
        {
            SentAt = DateTime.UtcNow;  
        }
    }
}
