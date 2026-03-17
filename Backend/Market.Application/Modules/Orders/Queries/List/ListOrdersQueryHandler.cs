namespace Market.Application.Modules.Orders.Queries.List
{
    public sealed class ListOrdersQueryHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<ListOrdersQuery, PageResult<ListOrdersQueryDto>>
    {
        public async Task<PageResult<ListOrdersQueryDto>> Handle(ListOrdersQuery request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this.");

            var q = context.Orders
                .AsNoTracking()
                .OrderByDescending(o => o.Date)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim().ToLower();

                q = q.Where(o =>
                    o.Id.ToString().Contains(search)
                    || o.User.Username.ToLower().Contains(search)
                    || o.User.Email.ToLower().Contains(search)
                    || o.OrderStatus.ToLower().Contains(search));
            }

            var projectedQuery = q.Select(o => new ListOrdersQueryDto
            {
                Id = o.Id,
                OrderDate = o.Date,
                Status = o.OrderStatus,
                TotalAmount = o.TotalAmount,
                User = new ListOrdersQueryUserDto
                {
                    Id = o.UserId,
                    Username = o.User.Username,
                    Email = o.User.Email,
                },
                Games = o.OrderItems
                    .OrderBy(oi => oi.Id)
                    .Select(g => new ListOrdersQueryGameDto
                    {
                        Id = g.GameId,
                        Name = g.Game.Name,
                        CoverImageURL = g.Game.CoverImageURL,
                        PublisherId = g.Game.PublisherId,
                        PublisherName = g.Game.Publisher.Name,
                        Price = g.Price,
                    })
                    .ToList(),
            });

            return await PageResult<ListOrdersQueryDto>.FromQueryableAsync(projectedQuery, request.Paging, ct);
        }
    }
}
