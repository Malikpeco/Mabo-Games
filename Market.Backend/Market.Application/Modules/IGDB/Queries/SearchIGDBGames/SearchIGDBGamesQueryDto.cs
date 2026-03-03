using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.IGDB.Queries.SearchIGDBGames
{
    public sealed class SearchIGDBGamesQueryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? CoverUrl { get; set; }
    }
}
