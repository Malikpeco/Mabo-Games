namespace Market.Application.Modules.Orders.Queries.List
{
    public sealed class ListOrdersQueryDto
    {

        public int Id { get; init; }
        public DateTime OrderDate { get; init; }
        public string Status { get; init; } = default!;
        public decimal TotalAmount { get; init; }
        public ListOrdersQueryUserDto User { get; init; } = default!;
        public IReadOnlyList<ListOrdersQueryGameDto> Games { get; init; } = [];



    }


    public sealed class ListOrdersQueryUserDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }
    }

    public sealed class ListOrdersQueryGameDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = default!;
        public string? CoverImageURL { get; init; }
        public int PublisherId { get; init; }
        public string PublisherName { get; init; } = default!;

        public decimal Price { get; init; }
    }


}
