using Market.Domain.Common;
using Market.Domain.Common.Attributes;
using Market.Domain.Entities.Identity;

namespace Market.Domain.Entities
{

    
    public class UserSecurityQuestionEntity : BaseEntity
    {
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        public int SecurityQuestionId { get; set; }
        public SecurityQuestionEntity SecurityQuestion { get; set; }

        [NoAudit]
        [PreserveString]
        public string AnswerHash { get; set; }
    }
}
