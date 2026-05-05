namespace Market.Application.Modules.Orders.Queries.GetById
{
    public sealed class GetOrderByIdQueryHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<GetOrderByIdQuery, GetOrderByIdQueryDto?>
    {
        public async Task<GetOrderByIdQueryDto?> Handle(GetOrderByIdQuery request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this.");

            return await context.Orders
                .AsNoTracking()
                .Where(o => o.Id == request.Id)
                .Select(o => new GetOrderByIdQueryDto
                {
                    Id = o.Id,
                    OrderDate = o.Date,
                    Status = o.OrderStatus,
                    TotalAmount = o.TotalAmount,
                    User = new GetOrderByIdQueryUserDto
                    {
                        Id = o.UserId,
                        Username = o.User.Username,
                        Email = o.User.Email,
                    },
                    Payment = o.Payment == null ? null : new GetOrderByIdPaymentDto
                    {
                        Id = o.Payment.Id,
                        PaymentStatus = o.Payment.PaymentStatus,
                        Total = o.Payment.Total,
                        Date = o.Payment.Date,
                        StripeCheckoutSessionId = o.Payment.StripeCheckoutSessionId,
                        StripePaymentIntentId = o.Payment.StripePaymentIntentId,
                    },
                    Games = o.OrderItems
                        .OrderBy(oi => oi.Id)
                        .Select(oi => new GetOrderByIdGameDto
                        {
                            Id = oi.GameId,
                            Name = oi.Game.Name,
                            CoverImageURL = oi.Game.CoverImageURL,
                            PublisherId = oi.Game.PublisherId,
                            PublisherName = oi.Game.Publisher.Name,
                            Price = oi.Price,
                        })
                        .ToList(),
                })
                .FirstOrDefaultAsync(ct);
        }
    }
}