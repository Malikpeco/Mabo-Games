using MediatR;
using Market.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Market.Application.Modules.Countries.Queries.List;

public sealed class ListCountriesQueryHandler(IAppDbContext ctx)
    : IRequestHandler<ListCountriesQuery, PageResult<ListCountriesQueryDto>>
{
    public async Task<PageResult<ListCountriesQueryDto>> Handle(
        ListCountriesQuery request, CancellationToken ct)
    {
        var q = ctx.Countries.AsNoTracking();


        var projectedQuery = q.OrderBy(x => x.Name)
            .Select(x => new ListCountriesQueryDto
            {
                Id = x.Id,
                Name = x.Name
            });

        return await PageResult<ListCountriesQueryDto>.FromQueryableAsync(projectedQuery, request.Paging, ct);
    }
}