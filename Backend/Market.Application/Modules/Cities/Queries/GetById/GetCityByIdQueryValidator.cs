using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Cities.Queries.GetById
{
    public sealed class GetCityByIdQueryValidator : AbstractValidator<GetCityByIdQuery>
    {
        public GetCityByIdQueryValidator() 
        {
            RuleFor(c => c.Id)
                .GreaterThan(0);
        }
    }
}
