using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Countries.Queries.ListCountriesAutocomplete
{
    public sealed class ListCountriesAutocompleteQuery : IRequest<List<ListCountriesAutocompleteQueryDto>>
    {
        public string Term { get; set; }
    }
}
