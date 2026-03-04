using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.UserSecurityQuestions.Commands.Remove
{
    public sealed class RemoveUserSecurityQuestionCommandValidator
        :AbstractValidator<RemoveUserSecurityQuestionCommand>       
    {

        public RemoveUserSecurityQuestionCommandValidator() {

            RuleFor(x => x.Id)
                 .GreaterThan(0)
                 .WithMessage("Id must be a positive value!");
        
        }
    }
}
