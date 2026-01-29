using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Genres.Queries.List
{
    public class ListGenresQueryHandler(IAppDbContext context)
        :IRequestHandler<ListGenresQuery, PageResult<ListGenresQueryDto>>
    {
        public async Task<PageResult<ListGenresQueryDto>> Handle(ListGenresQuery request, CancellationToken ct)
        {
            var q = context.Genres.AsNoTracking()
                .Select(g => new ListGenresQueryDto
                {
                    Id = g.Id,
                    Name = g.Name
                });

            return await PageResult<ListGenresQueryDto>.FromQueryableAsync(q, request.Paging, ct);

        }
    }
}
