namespace Market.Application.Modules.IGDB.Queries.GetIGDBGameDetails
{
    public sealed class GetIGDBGameDetailsQueryHandler(IAppCurrentUser currentUser,IIGDBService igdbService) : IRequestHandler<GetIGDBGameDetailsQuery, GetIGDBGameDetailsDto>
    {
        public async Task<GetIGDBGameDetailsDto> Handle(GetIGDBGameDetailsQuery request, CancellationToken ct)
        {
            //if (!currentUser.IsAdmin)
            //   throw new MarketForbiddenException();

            return await igdbService.GetGameDetailsAsync(request.GameId, ct);

        }
    }

}
