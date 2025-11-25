using Market.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Domain.Entities
{
    public class SecurityQuestionEntity : BaseEntity
    {
        public string Question { get; set; }
        public IReadOnlyCollection<UserSecurityQuestionEntity> UserSecurityQuestions { get; private set; } = new List<UserSecurityQuestionEntity>();
    }
}
