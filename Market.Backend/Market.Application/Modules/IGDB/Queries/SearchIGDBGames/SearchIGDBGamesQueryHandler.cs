
using Market.Application.Abstractions;
using Market.Application.Modules.IGDB.Queries.SearchIGDBGames;

public sealed class SearchIGDBGamesQueryHandler(IIGDBService igdbService, IAppCurrentUser appCurrentUser): IRequestHandler<SearchIGDBGamesQuery, List<SearchIGDBGamesQueryDto>>
{

    public async Task<List<SearchIGDBGamesQueryDto>> Handle(SearchIGDBGamesQuery request,CancellationToken cancellationToken)
    {
        //if(!appCurrentUser.IsAdmin)
         //   throw new MarketForbiddenException();

        return await igdbService.SearchGamesAsync(request.Search, cancellationToken);
    }
}

