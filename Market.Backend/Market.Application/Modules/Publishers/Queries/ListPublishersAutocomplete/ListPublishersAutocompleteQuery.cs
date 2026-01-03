using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Publishers.Queries.ListPublishersAutocomplete
{
    public sealed class ListPublishersAutocompleteQuery : IRequest<List<ListPublishersAutocompleteQueryDto>>
    {
        public string Term { get; set; }
    }
}
