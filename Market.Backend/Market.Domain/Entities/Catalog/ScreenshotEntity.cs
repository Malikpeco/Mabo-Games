using Market.Domain.Common;
using Market.Domain.Common.Attributes;

namespace Market.Domain.Entities
{
    [NoAudit]
    [PreserveString]
    public class ScreenshotEntity :BaseEntity
    {
        public string ImageURL { get; set; }
        public int GameId { get; set; }
        public GameEntity Game { get; set; }
    }
}
