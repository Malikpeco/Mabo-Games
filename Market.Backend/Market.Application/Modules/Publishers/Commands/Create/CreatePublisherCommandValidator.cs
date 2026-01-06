using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Publishers.Commands.Create
{
    public sealed class CreatePublisherCommandValidator : AbstractValidator<CreatePublisherCommand>
    {
        public CreatePublisherCommandValidator() 
        {
            RuleFor(p => p.Name)
                .NotEmpty();
            RuleFor(p => p.CountryId)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
