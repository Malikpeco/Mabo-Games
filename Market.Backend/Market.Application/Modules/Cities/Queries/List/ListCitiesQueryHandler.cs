using Market.Application.Modules.Cities.Queries.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Cities.Queries.List
{
    public sealed class ListCitiesQueryHandler(IAppDbContext context, IAppCurrentUser currentUser)
        :IRequestHandler<ListCitiesQuery, PageResult<ListCitiesQueryDto>>
    {
        public async Task<PageResult<ListCitiesQueryDto>> Handle(ListCitiesQuery request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this.");

            var q = context.Cities.AsNoTracking();

            if (!string.IsNullOrEmpty(request.Search))
            {
                q = q.Where(c => c.Name.ToLower().Contains((request.Search.Trim()).ToLower()));
            }

            if (request.CountryId.HasValue)
            {
                q=q.Where(c=>c.CountryId==request.CountryId.Value);
            }

            var projectedQuery = q.Select(c => new ListCitiesQueryDto
            {
                Id = c.Id,
                Name = c.Name,
                Country = new CountryDto
                {
                    Id = c.CountryId,
                    Name = c.Country.Name,
                }
            });

            return await PageResult<ListCitiesQueryDto>.FromQueryableAsync(projectedQuery,request.Paging, ct);

        }
    }
}
