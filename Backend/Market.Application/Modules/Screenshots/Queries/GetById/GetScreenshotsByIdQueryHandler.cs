namespace Market.Application.Modules.Screenshots.Queries.GetById
{
    public sealed class GetScreenshotsByIdQueryHandler : IRequestHandler<GetScreenshotsByIdQuery, GetScreenshotsByIdQueryDto>
    {
        public async Task<GetScreenshotsByIdQueryDto> Handle(GetScreenshotsByIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
