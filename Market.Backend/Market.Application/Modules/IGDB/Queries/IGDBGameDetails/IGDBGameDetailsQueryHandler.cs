
namespace Market.Application.Modules.IGDB.Queries.IGDBGameDetails
{
    public sealed class IGDBGameDetailsQueryHandler : IRequestHandler<IGDBGameDetailsQuery, IGDBGameDetailsDto>
    {
        public async Task<IGDBGameDetailsDto> Handle(IGDBGameDetailsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
