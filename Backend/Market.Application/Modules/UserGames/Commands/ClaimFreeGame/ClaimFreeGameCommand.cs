namespace Market.Application.Modules.UserGames.Commands.ClaimFreeGame
{
    public sealed class ClaimFreeGameCommand : IRequest<int>
    {
        public int GameId { get; set; }
    }
}