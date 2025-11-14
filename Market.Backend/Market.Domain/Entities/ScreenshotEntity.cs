using Market.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Domain.Entities
{
    public class ScreenshotEntity :BaseEntity
    {
        public string ImageURL { get; set; }
        public int GameId { get; set; }
        public GameEntity Game { get; set; }
    }
}
