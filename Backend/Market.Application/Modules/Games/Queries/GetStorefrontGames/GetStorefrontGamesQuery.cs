using Market.Application.Modules.Games.Dto;
using Market.Domain.Common.Attributes;
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

        [PreserveString] //Im not sure what this does but im just letting it do its thing
        public string? Sort { get; init; } 
        public List<int>? GenreIds{ get; init; }
    }
}
