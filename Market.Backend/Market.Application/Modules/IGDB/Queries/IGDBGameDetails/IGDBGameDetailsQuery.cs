namespace Market.Application.Modules.IGDB.Queries.IGDBGameDetails
{
    public sealed class IGDBGameDetailsQuery : IRequest<IGDBGameDetailsDto>
    {
        string Search { get; set; }
    }
}
