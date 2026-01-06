using Market.Domain.Common;
using Market.Domain.Common.Attributes;

namespace Market.Domain.Entities
{
    [PreserveString]
    public class SecurityQuestionEntity : BaseEntity
    {
        public string Question { get; set; }
        public IReadOnlyCollection<UserSecurityQuestionEntity> UserSecurityQuestions { get; private set; } = new List<UserSecurityQuestionEntity>();
    }
}
