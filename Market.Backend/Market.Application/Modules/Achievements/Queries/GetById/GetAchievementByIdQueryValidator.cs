using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Achievements.Queries.GetById
{
    public sealed class GetAchievementByIdQueryValidator : AbstractValidator<GetAchievementByIdQuery>
    {
        public GetAchievementByIdQueryValidator() 
        {
            RuleFor(a => a.Id)
                .GreaterThan(0);
        }
    }
}
