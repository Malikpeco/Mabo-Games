using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Carts.Commands
{
    public sealed class AddToCartCommandValidator : AbstractValidator<AddToCartCommand>
    {

        public AddToCartCommandValidator()
        {

            RuleFor(x => x.GameId).NotEmpty().WithMessage("GameId is required.");
        }
    }
}
