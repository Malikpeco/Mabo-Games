using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Reviews.Commands.Create
{
    public sealed class CreateReviewCommandValidator :AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator()
        {
            RuleFor(r => r.Rating)
                .InclusiveBetween(1, 5);
        }
    }
}
