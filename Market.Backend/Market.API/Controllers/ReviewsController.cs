using Market.Application.Modules.Reviews.Commands.Create;
using Market.Application.Modules.Reviews.Queries.List;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public sealed class ReviewsController(ISender sender):ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Create(CreateReviewCommand command, CancellationToken ct)
        {
            return Ok(await sender.Send(command, ct));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<PageResult<ListReviewsQueryDto>> List([FromQuery]ListReviewsQuery query, CancellationToken ct)
        {
            return await sender.Send(query, ct);
        }

    }
}
