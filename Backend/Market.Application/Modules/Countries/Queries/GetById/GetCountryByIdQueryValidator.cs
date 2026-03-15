using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Countries.Queries.GetById
{
    public sealed class GetCountryByIdQueryValidator : AbstractValidator<GetCountryByIdQuery>
    {
        public GetCountryByIdQueryValidator() 
        {
            RuleFor(c => c.Id)
                .GreaterThan(0);
        }
    }
}
