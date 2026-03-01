using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Genres.Commands.Create
{
    internal class CreateGenreCommandValidator:AbstractValidator<CreateGenreCommand>
    {

        public CreateGenreCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Genre name must not be empty!")
                .MinimumLength(2);
                
        }

    }
}
