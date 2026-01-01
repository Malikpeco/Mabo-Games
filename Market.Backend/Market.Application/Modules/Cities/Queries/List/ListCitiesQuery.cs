using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Cities.Queries.List
{
    public sealed class ListCitiesQuery : BasePagedQuery<ListCitiesQueryDto>
    {
        public string? Search {  get; set; }
        public int? CountryId { get; set; }
    }
}
