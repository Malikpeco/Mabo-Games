using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Games.Queries.ListGamesAutocompleteQuery
{
    public sealed class ListGamesAutocompleteQueryHandler(IAppDbContext context) 
        : IRequestHandler<ListGamesAutocompleteQuery,List<ListGamesAutocompleteQueryDto>>
    {
        public async Task<List<ListGamesAutocompleteQueryDto>> Handle(ListGamesAutocompleteQuery request, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(request.Term))
                return new List<ListGamesAutocompleteQueryDto>();

            var term = request.Term.Trim().ToLower();

            var query = context.Games
                .AsNoTracking()
                .Where(g => g.Name.ToLower().Contains(term));

            if (request.GenreId.HasValue)
            {
                query = query.Where(g => g.GameGenres.Any(gg => gg.GenreId == request.GenreId.Value));
            }

            var projectedQuery = await query.OrderBy(g=>!g.Name.ToLower().StartsWith(term)).Select(g => new ListGamesAutocompleteQueryDto
            {
                Id = g.Id,
                Name = g.Name,
            }).Take(10).ToListAsync(ct);

            return projectedQuery;

        }
    }
}
