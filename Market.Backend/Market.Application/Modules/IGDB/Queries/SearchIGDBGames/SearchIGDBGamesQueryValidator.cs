using System.ComponentModel.DataAnnotations;

namespace Market.Application.Modules.IGDB.Queries.SearchIGDBGames
{
    public sealed class SearchIGDBGamesQueryValidator : AbstractValidator<SearchIGDBGamesQuery>
    {
        public SearchIGDBGamesQueryValidator()
        {
            RuleFor(x => x.Search)
                .NotEmpty()
                .WithMessage("Please enter a search topic");
                
        }
    }
}
