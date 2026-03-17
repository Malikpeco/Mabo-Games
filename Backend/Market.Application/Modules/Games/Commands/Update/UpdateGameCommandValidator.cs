using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Games.Commands.Update
{
    public sealed class UpdateGameCommandValidator : AbstractValidator<UpdateGameCommand>
    {
        public UpdateGameCommandValidator()
        {
            RuleFor(g => g.Name)
                .NotEmpty();

            RuleFor(g => g.Price)
                .GreaterThanOrEqualTo(0);

            RuleFor(g => g.GenreIds)
                .NotEmpty().WithMessage("At least one genre must be selected.");

            RuleForEach(g => g.GenreIds)
                .GreaterThan(0);

            RuleForEach(g => g.ScreenshotUrls)
                .Matches(@"^https?://.*$")
                .WithMessage("Must be a valid URL.");
        }
    }
}
