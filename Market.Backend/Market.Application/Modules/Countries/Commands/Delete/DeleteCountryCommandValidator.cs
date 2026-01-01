using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Countries.Commands.Delete
{
    public sealed class DeleteCountryCommandValidator : AbstractValidator<DeleteCountryCommand>
    {
        public DeleteCountryCommandValidator() 
        {
            RuleFor(c => c.Id)
                .GreaterThan(0);
        }
    }
}
