using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Games.Commands.Update
{
    public sealed class UpdateGameCommandValidator : AbstractValidator<UpdateGameCommand>
    {
        private const long _maxFileSize = 5 * 1024 * 1024; // 5MB
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

            RuleFor(x => x.File.Length)
                    .GreaterThan(0)
                    .WithMessage("File cannot be empty")
                    .LessThanOrEqualTo(_maxFileSize)
                    .WithMessage("File size must not exceed 5MB");

        }
    }
}
