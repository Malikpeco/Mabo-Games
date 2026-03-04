using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Cities.Queries.GetById
{
    public sealed class GetCityByIdQuery : IRequest<GetCityByIdQueryDto>
    {
        public int Id { get; set; }
    }
}
