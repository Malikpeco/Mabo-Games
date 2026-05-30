using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Games.Commands.Create
{
    public sealed class CreateGameCommandValidator : AbstractValidator<CreateGameCommand>
    {

        private const long _maxFileSize = 5 * 1024 * 1024; // 5MB
        public CreateGameCommandValidator()
        {
            RuleFor(g => g.Name)
                .NotEmpty();

            RuleFor(g => g.Price)
                .GreaterThanOrEqualTo(0);

            RuleFor(g => g.ReleaseDate)
                .LessThan(DateTime.UtcNow.AddDays(1));

            RuleFor(g => g.PublisherId)
                .GreaterThan(0);

            RuleFor(g => g.GenreIds)
             .NotEmpty().WithMessage("At least one genre must be selected.");

            RuleFor(g => g.CoverImageURL)
                .NotEmpty().Matches(@"^https?://.*$")
                .WithMessage("Must be a valid URL.");

            RuleFor(x => x.File.Length)
        .GreaterThan(0)
        .WithMessage("File cannot be empty")
        .LessThanOrEqualTo(_maxFileSize)
        .WithMessage("File size must not exceed 5MB");


        }
    }
}
