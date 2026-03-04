using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Publishers.Queries.ListPublishersAutocomplete
{
    public sealed class ListPublishersAutocompleteQueryHandler(IAppCurrentUser currentUser, IAppDbContext context)
        :IRequestHandler<ListPublishersAutocompleteQuery, List<ListPublishersAutocompleteQueryDto>>
    {
        public async Task<List<ListPublishersAutocompleteQueryDto>> Handle(ListPublishersAutocompleteQuery request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this.");

            if(string.IsNullOrWhiteSpace(request.Term))
                return new List<ListPublishersAutocompleteQueryDto>();

            var term = request.Term.ToLower().Trim();

            var query = await context.Publishers.AsNoTracking()
                .Where(p => p.Name.ToLower().Contains(term))
                .OrderBy(p => !p.Name.ToLower().StartsWith(term))
                .Select(p => new ListPublishersAutocompleteQueryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                })
                .ToListAsync(ct);

            return query;
        }
    }
}
