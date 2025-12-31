using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Achievements.Commands.Update
{
    public sealed class UpdateAchievementCommandValidator : AbstractValidator<UpdateAchievementCommand>
    {
        public UpdateAchievementCommandValidator() 
        {
            RuleFor(a => a.Name)
                .NotEmpty();
            RuleFor(a => a.ImageURL)
                .NotEmpty();
        }
    }
}
