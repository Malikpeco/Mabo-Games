using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.IGDB.Queries.IGDBGameDetails
{
    public sealed class IGDBGameDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Genres { get; set; } = new();
        public string CoverUrl { get; set; }
        public List<string> ScreenshotUrls { get; set; } = new();

    }
}
