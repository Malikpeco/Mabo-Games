using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Cities.Commands.Create
{
    public sealed class CreateCityCommandValidator : AbstractValidator<CreateCityCommand>
    {
        public CreateCityCommandValidator() 
        {
            RuleFor(c => c.CountryId)
                .GreaterThan(0);
            RuleFor(c => c.Name)
                .NotEmpty();
        }
    }
}
