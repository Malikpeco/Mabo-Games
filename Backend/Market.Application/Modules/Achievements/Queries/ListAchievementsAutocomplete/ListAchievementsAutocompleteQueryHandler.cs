using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Achievements.Queries.ListAchievementsAutocomplete
{
    public sealed class ListAchievementsAutocompleteQueryHandler(IAppDbContext context, IAppCurrentUser currentUser)
        :IRequestHandler<ListAchievementsAutocompleteQuery, List<ListAchievementsAutocompleteQueryDto>>
    {
        public async Task<List<ListAchievementsAutocompleteQueryDto>> Handle(ListAchievementsAutocompleteQuery request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this.");

            if (string.IsNullOrWhiteSpace(request.Term))
                return new List<ListAchievementsAutocompleteQueryDto>();

            var term = request.Term.ToLower().Trim();

            var query = context.Achievements
                .AsNoTracking()
                .Where(a => a.Name.ToLower().Contains(term));

            var projectedQuery = await query.OrderBy(s=>!s.Name.StartsWith(term))
                .Select(a => new ListAchievementsAutocompleteQueryDto
                {
                    Id = a.Id,
                    Name = a.Name,
                }).Take(10).ToListAsync(ct);

            return projectedQuery;

        }
    }
}
