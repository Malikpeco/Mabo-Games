using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Publishers.Queries.List
{
    public sealed class ListPublishersQueryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PublisherCountryDto Country { get; set; }
        public IReadOnlyList<PublishersGameDto> Games { get; set; } = new List<PublishersGameDto>();

    }

    public sealed class PublisherCountryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public sealed class PublishersGameDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
