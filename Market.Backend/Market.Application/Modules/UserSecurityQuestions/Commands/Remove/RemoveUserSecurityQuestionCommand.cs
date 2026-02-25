using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.UserSecurityQuestions.Commands.Remove
{
    public sealed class RemoveUserSecurityQuestionCommand:IRequest<int>
    {

        [JsonIgnore]
        public int Id {get;set;}


    }
}
