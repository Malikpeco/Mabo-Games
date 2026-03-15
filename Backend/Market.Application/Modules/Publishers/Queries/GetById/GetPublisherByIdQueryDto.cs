using Market.Application.Modules.Publishers.Queries.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Publishers.Queries.GetById
{
    public sealed class GetPublisherByIdQueryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PublisherCountryDto Country { get; set; }
        public IReadOnlyList<PublishersGameDto> Games { get; set; } = new List<PublishersGameDto>();

    }
}
