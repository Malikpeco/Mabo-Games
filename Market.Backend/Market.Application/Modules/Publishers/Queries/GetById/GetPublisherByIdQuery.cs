using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Publishers.Queries.GetById
{
    public sealed class GetPublisherByIdQuery : IRequest<GetPublisherByIdQueryDto>
    {
        public int Id { get; set; }
    }
}
