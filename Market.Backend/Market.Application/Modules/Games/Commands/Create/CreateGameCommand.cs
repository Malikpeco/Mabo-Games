using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Games.Commands.Create
{
    public sealed class CreateGameCommand : IRequest<int>//when this command is handled it returns an int (gameId)
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int PublisherId { get; set; }
        public string? CoverImageURL { get; set; }
        public List<int> GenreIds { get; set; } = new(); 
        public List<string> ScreenshotUrls { get; set; } = new(); 

    }
}
