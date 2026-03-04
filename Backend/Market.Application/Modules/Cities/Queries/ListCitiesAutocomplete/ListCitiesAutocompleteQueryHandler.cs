using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Cities.Queries.ListCitiesAutocomplete
{
    public sealed class ListCitiesAutocompleteQueryHandler(IAppDbContext context, IAppCurrentUser currentUser) 
        : IRequestHandler<ListCitiesAutocompleteQuery, List<ListCitiesAutocompleteQueryDto>>
    {
        public async Task<List<ListCitiesAutocompleteQueryDto>> Handle(ListCitiesAutocompleteQuery request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this.");

            if (string.IsNullOrWhiteSpace(request.Term))
                return new List<ListCitiesAutocompleteQueryDto>();

            var term = request.Term.ToLower().Trim();

            var query = await context.Cities.AsNoTracking()
                .Where(c => c.Name.ToLower().Contains(term))
                .Select(c=>new ListCitiesAutocompleteQueryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .OrderBy(c => !c.Name.ToLower().StartsWith(term))
                .Take(10)
                .ToListAsync(ct);

            return query;

        }
    }
}
