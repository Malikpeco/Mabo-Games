using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Countries.Queries.GetById
{
    public sealed class GetCountryByIdQuery : IRequest<GetCountryByIdQueryDto>
    {
        public int Id { get; set; }
    }
}
