namespace Market.Application.Modules.Dashboard.Queries.GetSummary
{
    public sealed class GetDashboardSummaryQueryHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<GetDashboardSummaryQuery, GetDashboardSummaryQueryDto>
    {
        private const string PaidStatus = "Paid";
        private const int DefaultRevenueTrendDays = 30;
        private const int TopSellingGamesLimit = 5;
        private const int RecentOrdersLimit = 10;

        public async Task<GetDashboardSummaryQueryDto> Handle(GetDashboardSummaryQuery request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new MarketForbiddenException();

            var trendEnd = (request.ToDate ?? DateTime.UtcNow).Date;
            var trendStart = (request.FromDate?.Date) ?? trendEnd.AddDays(-(DefaultRevenueTrendDays - 1));
            if (trendStart > trendEnd)
            {
                (trendStart, trendEnd) = (trendEnd, trendStart);
            }

            var weekAgo = DateTime.UtcNow.AddDays(-7);

            var paidOrders = context.Orders.AsNoTracking().Where(o => o.OrderStatus == PaidStatus);

            var totalRevenue = await paidOrders.SumAsync(o => (decimal?)o.TotalAmount, ct) ?? 0m;
            var totalOrders = await paidOrders.CountAsync(ct);
            var ordersThisWeek = await paidOrders.CountAsync(o => o.Date >= weekAgo, ct);
            var totalUsers = await context.Users.AsNoTracking().CountAsync(ct);
            var totalGames = await context.Games.AsNoTracking().CountAsync(ct);

            var topSellingGames = await context.OrderItems
                .AsNoTracking()
                .Where(oi => oi.Order.OrderStatus == PaidStatus)
                .GroupBy(oi => new { oi.GameId, oi.Game.Name, oi.Game.CoverImageURL })
                .Select(g => new TopSellingGameDto
                {
                    GameId = g.Key.GameId,
                    Name = g.Key.Name,
                    CoverImageURL = g.Key.CoverImageURL,
                    UnitsSold = g.Count(),
                    Revenue = g.Sum(oi => oi.Price),
                })
                .OrderByDescending(g => g.UnitsSold)
                .ThenByDescending(g => g.Revenue)
                .Take(TopSellingGamesLimit)
                .ToListAsync(ct);

            var recentOrders = await context.Orders
                .AsNoTracking()
                .OrderByDescending(o => o.Date)
                .Take(RecentOrdersLimit)
                .Select(o => new RecentOrderDto
                {
                    Id = o.Id,
                    Date = o.Date,
                    Username = o.User.Username,
                    TotalAmount = o.TotalAmount,
                    Status = o.OrderStatus,
                })
                .ToListAsync(ct);

            var trendEndExclusive = trendEnd.AddDays(1);
            var revenueByDayRaw = await paidOrders
                .Where(o => o.Date >= trendStart && o.Date < trendEndExclusive)
                .GroupBy(o => o.Date.Date)
                .Select(g => new { Date = g.Key, Revenue = g.Sum(o => o.TotalAmount) })
                .ToListAsync(ct);

            var revenueLookup = revenueByDayRaw.ToDictionary(x => x.Date, x => x.Revenue);
            var trendDayCount = (int)(trendEnd - trendStart).TotalDays + 1;
            var revenueByDay = Enumerable.Range(0, trendDayCount)
                .Select(offset => trendStart.AddDays(offset))
                .Select(date => new RevenueByDayDto
                {
                    Date = date,
                    Revenue = revenueLookup.TryGetValue(date, out var revenue) ? revenue : 0m,
                })
                .ToList();

            return new GetDashboardSummaryQueryDto
            {
                TotalRevenue = totalRevenue,
                TotalOrders = totalOrders,
                OrdersThisWeek = ordersThisWeek,
                TotalUsers = totalUsers,
                TotalGames = totalGames,
                TopSellingGames = topSellingGames,
                RecentOrders = recentOrders,
                RevenueByDay = revenueByDay,
            };
        }
    }
}
