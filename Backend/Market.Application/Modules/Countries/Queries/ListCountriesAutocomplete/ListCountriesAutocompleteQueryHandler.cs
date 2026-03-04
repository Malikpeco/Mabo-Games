using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Countries.Queries.ListCountriesAutocomplete
{
    public sealed class ListCountriesAutocompleteQueryHandler(IAppCurrentUser currentUser, IAppDbContext context)
        :IRequestHandler<ListCountriesAutocompleteQuery, List<ListCountriesAutocompleteQueryDto>>
    {
        public async Task<List<ListCountriesAutocompleteQueryDto>> Handle(ListCountriesAutocompleteQuery request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this.");

            if(string.IsNullOrWhiteSpace(request.Term))
                return new List<ListCountriesAutocompleteQueryDto>();

            var term = request.Term.ToLower().Trim();

            var q = await context.Countries.AsNoTracking().Where(c => c.Name.ToLower().Contains(term))
                .OrderBy(c => !c.Name.ToLower().StartsWith(term))
                .Select(c => new ListCountriesAutocompleteQueryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .Take(10).ToListAsync(ct);

            return q;
        }
    }
}
