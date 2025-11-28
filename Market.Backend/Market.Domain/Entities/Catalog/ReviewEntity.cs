using Market.Domain.Common;

namespace Market.Domain.Entities
{
    public class ReviewEntity : BaseEntity
    {
        public string? Content { get; set; }
        public float Rating { get; set; }
        public DateTime Date { get; set; }
        public int UserGameId { get; set; }
        public UserGameEntity UserGame { get; set; }

        public ReviewEntity()
        {
            Date = DateTime.UtcNow; // set review date automatically
        }
    }
}
