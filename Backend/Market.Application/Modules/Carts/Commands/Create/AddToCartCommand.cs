namespace Market.Application.Modules.Carts.Commands.Create
{
    public sealed class AddToCartCommand : IRequest<Unit>
    {
        required public int GameId { get; set; }
    }
}
