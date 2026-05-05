using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Genres.Queries.List
{
    public sealed class ListGenresQuery : BasePagedQuery<ListGenresQueryDto>
    {
        public string? Search { get; set; }

    }
}
