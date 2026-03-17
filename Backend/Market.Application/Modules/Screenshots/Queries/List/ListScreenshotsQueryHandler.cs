namespace Market.Application.Modules.Screenshots.Queries.List
{
    public sealed class ListScreenshotsQueryHandler : IRequestHandler<ListScreenshotsQuery, PageResult<ListScreenshotsQueryDto>>
    {
        public async Task<PageResult<ListScreenshotsQueryDto>> Handle(ListScreenshotsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
