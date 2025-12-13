using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.SecurityQuestions.Dto
{
    public sealed class UpdateSecurityQuestionResultDto
    {

        public int Id { get; set; }

        public string OldQuestion {  get; set; }
        public string NewQuestion { get; set; }

    }
}
