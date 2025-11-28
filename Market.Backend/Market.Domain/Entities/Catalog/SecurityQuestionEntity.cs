using Market.Domain.Common;

namespace Market.Domain.Entities
{
    public class SecurityQuestionEntity : BaseEntity
    {
        public string Question { get; set; }
        public IReadOnlyCollection<UserSecurityQuestionEntity> UserSecurityQuestions { get; private set; } = new List<UserSecurityQuestionEntity>();
    }
}
