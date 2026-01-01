using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Achievements.Commands.Create
{
    public sealed class CreateAchievementCommandValidator : AbstractValidator<CreateAchievementCommand>
    {
        public CreateAchievementCommandValidator() {
            RuleFor(a => a.Name)
                .NotEmpty();
            RuleFor(a => a.ImageURL)
                .NotEmpty();
        }
    }
}
