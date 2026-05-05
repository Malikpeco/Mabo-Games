namespace Market.Application.Modules.Orders.Queries.GetById
{
    public sealed class GetOrderByIdQueryDto
    {
        public int Id { get; init; }
        public DateTime OrderDate { get; init; }
        public string Status { get; init; } = default!;
        public decimal TotalAmount { get; init; }
        public GetOrderByIdQueryUserDto User { get; init; } = default!;
        public GetOrderByIdPaymentDto? Payment { get; init; }
        public IReadOnlyList<GetOrderByIdGameDto> Games { get; init; } = [];
    }

    public sealed class GetOrderByIdQueryUserDto
    {
        public int Id { get; init; }
        public string Username { get; init; } = default!;
        public string Email { get; init; } = default!;
    }

    public sealed class GetOrderByIdPaymentDto
    {
        public int Id { get; init; }
        public string PaymentStatus { get; init; } = default!;
        public decimal Total { get; init; }
        public DateTime Date { get; init; }
        public string? StripeCheckoutSessionId { get; init; }
        public string? StripePaymentIntentId { get; init; }
    }

    public sealed class GetOrderByIdGameDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = default!;
        public string? CoverImageURL { get; init; }
        public int PublisherId { get; init; }
        public string PublisherName { get; init; } = default!;
        public decimal Price { get; init; }
    }
}