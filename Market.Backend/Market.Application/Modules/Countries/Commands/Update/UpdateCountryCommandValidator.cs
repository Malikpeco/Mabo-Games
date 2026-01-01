using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Countries.Commands.Update
{
    public sealed class UpdateCountryCommandValidator : AbstractValidator<UpdateCountryCommand>
    {
        public UpdateCountryCommandValidator() 
        {
            RuleFor(c => c.Name)
                .NotEmpty();

        }
    }
}
