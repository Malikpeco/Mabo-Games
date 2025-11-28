using Market.Domain.Common;
using Market.Domain.Entities.Identity;

namespace Market.Domain.Entities
{
    public class UserSecurityQuestionEntity : BaseEntity
    {
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        public int SecurityQuestionId { get; set; }
        public SecurityQuestionEntity SecurityQuestion { get; set; }
        public string AnswerHash { get; set; }
    }
}
