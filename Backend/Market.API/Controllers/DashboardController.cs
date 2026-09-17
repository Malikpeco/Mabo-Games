using Market.Application.Modules.Dashboard.Queries.GetSummary;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    [Authorize]
    public class DashboardController(ISender sender) : ControllerBase
    {
        [HttpGet("summary")]
        public async Task<GetDashboardSummaryQueryDto> GetSummary([FromQuery] GetDashboardSummaryQuery query, CancellationToken ct)
        {
            return await sender.Send(query, ct);
        }
    }
}
