using Market.Domain.Common;

namespace Market.Domain.Entities
{
    public class ScreenshotEntity :BaseEntity
    {
        public string ImageURL { get; set; }
        public int GameId { get; set; }
        public GameEntity Game { get; set; }
    }
}
