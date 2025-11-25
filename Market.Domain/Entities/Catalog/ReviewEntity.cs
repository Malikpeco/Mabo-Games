using Market.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Domain.Entities
{
    public class ReviewEntity : BaseEntity
    {
        public string? Content { get; set; }
        public float Rating { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }
        public UserGameEntity UserGame { get; set; }

        public ReviewEntity()
        {
            Date = DateTime.UtcNow; // set review date automatically
        }
    }
}
