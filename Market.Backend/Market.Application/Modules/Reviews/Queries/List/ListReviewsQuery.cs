using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Reviews.Queries.List
{
    public sealed class ListReviewsQuery : BasePagedQuery<ListReviewsQueryDto>
    {
        //this query can be used for finding specific-game reviews, or all reviews in the db.
        public int? GameId { get; set; }
    }
}
