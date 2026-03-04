using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Cities.Queries.ListCitiesAutocomplete
{
    public sealed class ListCitiesAutocompleteQuery : IRequest<List<ListCitiesAutocompleteQueryDto>>
    {
        public string Term { get; set; }
    }
}
