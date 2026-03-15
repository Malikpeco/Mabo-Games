using Market.Application.Modules.Carts.Commands.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Carts.Commands.Delete
{
    public sealed class RemoveFromCartCommandValidator : AbstractValidator<RemoveFromCartCommand>
    {

        public RemoveFromCartCommandValidator()
        {

            RuleFor(x => x.GameId).NotEmpty().WithMessage("GameId is required.");
            RuleFor(x => x.GameId).GreaterThan(0);
        }
    }
}
