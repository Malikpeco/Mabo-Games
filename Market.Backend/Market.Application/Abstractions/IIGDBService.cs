using Market.Application.Common.IGDB;
using Market.Application.Modules.IGDB.Queries.SearchIGDBGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Abstractions
{
    public interface IIGDBService
    {
        Task<string> getTokenAsync(CancellationToken ct);
        Task<List<SearchIGDBGamesQueryDto>> SearchGamesAsync(string searchTerm,CancellationToken ct);
    }
}
