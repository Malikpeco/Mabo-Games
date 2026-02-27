using Market.Application.Modules.IGDB.Queries.IGDBGameDetails;

namespace Market.Application.Modules.IGDB.Queries.SearchIGDBGames
{
    public record SearchIGDBGamesQuery(string Search) : IRequest<List<SearchIGDBGamesQueryDto>>;
}
