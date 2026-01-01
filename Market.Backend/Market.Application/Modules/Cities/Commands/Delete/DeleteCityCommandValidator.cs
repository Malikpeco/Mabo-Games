using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Cities.Commands.Delete
{
    public sealed class DeleteCityCommandValidator : AbstractValidator<DeleteCityCommand>
    {
        public DeleteCityCommandValidator() 
        {
            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("Id must be greater than 0.");
        }
    }
}
