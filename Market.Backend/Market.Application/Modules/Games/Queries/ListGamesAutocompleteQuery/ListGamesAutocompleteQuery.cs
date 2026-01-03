using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Games.Queries.ListGamesAutocompleteQuery
{
    public sealed class ListGamesAutocompleteQuery : IRequest<List<ListGamesAutocompleteQueryDto>>
    {
        public string Term { get; set; }
        public int? GenreId { get; set; }
    }
}
