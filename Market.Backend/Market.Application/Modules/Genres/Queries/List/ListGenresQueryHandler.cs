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
    public class ListGenresQueryHandler(IAppDbContext context)
        :IRequestHandler<ListGenresQuery, List<ListGenresQueryDto>>
    {
        public async Task<List<ListGenresQueryDto>> Handle(ListGenresQuery request, CancellationToken ct)
        {
            var q = context.Genres.
            AsNoTracking();


            return await
                q.
                Select(x => new ListGenresQueryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                }
                ).
                ToListAsync(ct);

        }
    }
}
