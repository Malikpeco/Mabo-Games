using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Games.Queries.GetStorefrontGames
{
    public sealed class GetStorefrontGamesQueryValidator : AbstractValidator<GetStorefrontGamesQuery>
    {
        public GetStorefrontGamesQueryValidator() 
        {

            RuleForEach(x => x.GenreIds)
                .GreaterThan(0)
                .WithMessage("GenreId must be greated than 0!");
        }
    }
}
