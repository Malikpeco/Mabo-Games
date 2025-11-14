using Market.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
