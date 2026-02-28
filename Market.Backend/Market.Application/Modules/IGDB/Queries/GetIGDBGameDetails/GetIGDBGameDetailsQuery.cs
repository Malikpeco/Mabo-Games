namespace Market.Application.Modules.IGDB.Queries.GetIGDBGameDetails
{
    public sealed class GetIGDBGameDetailsQuery : IRequest<GetIGDBGameDetailsDto>
    {
        public int GameId { get; set; }
    }
}
