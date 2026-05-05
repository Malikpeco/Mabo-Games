using Market.Application.Abstractions;
using Market.Application.Modules.UserSecurityQuestions.Queries.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Genres.Queries.List
{
    public sealed class ListGenresQueryHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<ListGenresQuery, PageResult<ListGenresQueryDto>>
    {
        public async Task<PageResult<ListGenresQueryDto>> Handle(ListGenresQuery request, CancellationToken ct)
        {


            var q = context.Genres.AsNoTracking();

            var searchTerm = request.Search?.Trim().ToLower() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                q = q.Where(g => g.Name.ToLower().Contains(searchTerm));
            }


            var projectedQuery = q
                .Select(x => new ListGenresQueryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    GameCount = x.GameGenres.Count,
                }
                );

            return await PageResult<ListGenresQueryDto>.FromQueryableAsync(projectedQuery, request.Paging, ct);

        }
    }
}
