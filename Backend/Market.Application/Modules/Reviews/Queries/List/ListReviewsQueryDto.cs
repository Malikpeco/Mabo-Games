using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Reviews.Queries.List
{
    //reviews for a singular game
    public sealed class ListReviewsQueryDto
    {
        public string? Content { get; set; }
        public float Rating { get; set; }
        public DateTime Date { get; set; }
        public ReviewUserGameDto UserGame { get; set; }
    }

    public class ReviewUserGameDto
    {
        public int Id { get; set; }
        public ReviewUserDto User { get; set; }
        public ReviewGameDto Game { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
    public class ReviewUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }

    public class ReviewGameDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
