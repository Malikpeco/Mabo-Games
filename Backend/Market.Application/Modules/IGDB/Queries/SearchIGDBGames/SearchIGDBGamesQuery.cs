using Market.Application.Modules.IGDB.Queries.SearchIGDBGames;

namespace Market.Application.Modules.IGDB.Queries.SearchIGDBGames
{
    public record SearchIGDBGamesQuery(string Search) : IRequest<List<SearchIGDBGamesQueryDto>>;
}
