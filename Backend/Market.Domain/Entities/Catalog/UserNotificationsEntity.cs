using Market.Domain.Common.Attributes;
using Market.Domain.Common;
using Market.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Domain.Entities.Catalog
{
    public class UserNotificationsEntity : BaseEntity
    {
        public int UserId { get; set; }
       
        public UserEntity User { get; set; }

        public int NotificationId { get; set; }
        public NotificationEntity Notification { get; set; }

        public bool IsRead { get; set; }   

    }


}
