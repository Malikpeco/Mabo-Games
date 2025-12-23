using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Games.Dto
{
    public sealed class StorefrontGameDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = default!;
        public decimal Price { get; init; }
        public DateTime ReleaseDate { get; init; }
        public string? CoverImageURL { get; init; }
        public int PublisherId { get; init; }
        public string PublisherName { get; init; } = default!;
    }
}
