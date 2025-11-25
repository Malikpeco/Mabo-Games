using MediatR;
using System.Collections.Generic;

namespace Market.Application.Modules.Countries.Queries.List
{
    public sealed class ListCountriesQuery : BasePagedQuery<ListCountriesQueryDto>
    {
    }
}