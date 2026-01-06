using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Publishers.Queries.List
{
    public sealed class ListPublishersQueryHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<ListPublishersQuery, PageResult<ListPublishersQueryDto>>
    {
        public async Task<PageResult<ListPublishersQueryDto>> Handle(ListPublishersQuery request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this.");

            var q = context.Publishers.AsNoTracking();

            if (!string.IsNullOrEmpty(request.Search))
            {
                q = q.Where(p => p.Name.ToLower().Contains(request.Search.ToLower()));
            }

            //if(request.CountryId is not null)
            //{
            //    q = q.Where(p => p.CountryId == request.CountryId.Value);
            //}

            var projectedQuery = q
                .Select(p => new ListPublishersQueryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Country = new PublisherCountryDto
                    {
                        Id = p.Country.Id,
                        Name = p.Country.Name,
                    },
                    Games = p.Games.Select(g => new PublishersGameDto
                    {
                        Id = g.Id,
                        Name = g.Name,
                    }).ToList()
                });

            if (projectedQuery is null)
                throw new MarketNotFoundException("No publishers found.");


            return await PageResult<ListPublishersQueryDto>.FromQueryableAsync(projectedQuery, request.Paging, ct);

        }
    }
}
