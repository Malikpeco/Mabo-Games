namespace Market.Application.Modules.Dashboard.Queries.GetSummary
{
    public sealed class GetDashboardSummaryQuery : IRequest<GetDashboardSummaryQueryDto>
    {
        // Scopes only the revenue trend series below - KPIs, top sellers, and recent
        // orders always stay all-time. Defaults to the last 30 days when omitted.
        public DateTime? FromDate { get; init; }
        public DateTime? ToDate { get; init; }
    }
}
