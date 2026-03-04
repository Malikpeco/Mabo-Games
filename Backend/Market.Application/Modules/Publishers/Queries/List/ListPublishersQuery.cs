using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Publishers.Queries.List
{
    public sealed class ListPublishersQuery : BasePagedQuery<ListPublishersQueryDto>
    {
        public string? Search { get; set; }
    }
}
