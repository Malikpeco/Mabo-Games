namespace Market.Application.Modules.Countries.Queries.List;

public sealed class ListCountriesQueryHandler(IAppDbContext context, IAppCurrentUser currentUser)
    : IRequestHandler<ListCountriesQuery, PageResult<ListCountriesQueryDto>>
{
    public async Task<PageResult<ListCountriesQueryDto>> Handle(
        ListCountriesQuery request, CancellationToken ct)
    {
        if (!currentUser.IsAdmin)
            throw new Exception("You must be an admin to do this!");

        var q = context.Countries.AsNoTracking();

        var searchTerm = request.Search;

        if (!string.IsNullOrEmpty(searchTerm))
        {
            q = q.Where(c => c.Name.ToLower().Contains(searchTerm.ToLower()));
        }

        var projectedQuery = q.OrderBy(x => x.Name)
            .Select(x => new ListCountriesQueryDto
            {
                Id = x.Id,
                Name = x.Name
            });

        return await PageResult<ListCountriesQueryDto>.FromQueryableAsync(projectedQuery, request.Paging, ct);
    }
}