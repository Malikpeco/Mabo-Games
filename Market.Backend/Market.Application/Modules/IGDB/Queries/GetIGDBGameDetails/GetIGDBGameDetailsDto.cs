using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.IGDB.Queries.GetIGDBGameDetails
{
    public sealed class GetIGDBGameDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string? Summary { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? CoverUrl { get; set; }
        public List<string> Screenshots { get; set; }
        public List<string> Genres { get; set; }
        public string? Publisher { get; set; }

    }
}
