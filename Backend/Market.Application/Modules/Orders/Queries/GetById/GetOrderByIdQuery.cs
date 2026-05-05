namespace Market.Application.Modules.Orders.Queries.GetById
{
    public sealed class GetOrderByIdQuery : IRequest<GetOrderByIdQueryDto?>
    {
        public int Id { get; init; }
    }
}