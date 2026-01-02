using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Publishers.Commands.Update
{
    public sealed class UpdatePublisherCommandValidator : AbstractValidator<UpdatePublisherCommand>
    {
        public UpdatePublisherCommandValidator() 
        {
            RuleFor(p => p.Name)
                .NotEmpty();

            RuleFor(p => p.CountryId)
                .NotEmpty()
                .GreaterThan(0);
        }    
    }
}
