using Market.Application.Modules.Games.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Games.Queries.GetStorefrontGames
{
    public sealed class GetStorefrontGamesQuery : BasePagedQuery<StorefrontGameDto>
    {
        public string? Search { get; init; }
        public string? Sort { get; init; } 
        public List<int>? GenreIds{ get; init; }
    }
}
