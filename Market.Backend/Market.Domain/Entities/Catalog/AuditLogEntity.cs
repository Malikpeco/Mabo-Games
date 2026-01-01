using Market.Domain.Common;
using Market.Domain.Common.Attributes;
using Market.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Domain.Entities.Catalog
{

    [NoAudit]
    [PreserveString]
    public  class AuditLogEntity:BaseEntity
    {

        public int? UserId { get; set; }

        public UserEntity? User { get; set; }

        public string EntityName { get; set; }

        public string? EntityId { get; set; }   

        public string? BeforeChange { get; set; }

        public string? AfterChange { get; set; }

        public string Action { get; set; }




    }
}
