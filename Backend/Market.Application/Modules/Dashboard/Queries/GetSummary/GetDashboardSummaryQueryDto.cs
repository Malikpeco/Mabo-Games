namespace Market.Application.Modules.Dashboard.Queries.GetSummary
{
    public sealed class GetDashboardSummaryQueryDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int OrdersThisWeek { get; set; }
        public int TotalUsers { get; set; }
        public int TotalGames { get; set; }
        public List<TopSellingGameDto> TopSellingGames { get; set; } = new();
        public List<RecentOrderDto> RecentOrders { get; set; } = new();
        public List<RevenueByDayDto> RevenueByDay { get; set; } = new();
    }

    public sealed class TopSellingGameDto
    {
        public int GameId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? CoverImageURL { get; set; }
        public int UnitsSold { get; set; }
        public decimal Revenue { get; set; }
    }

    public sealed class RecentOrderDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Username { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public sealed class RevenueByDayDto
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
    }
}
