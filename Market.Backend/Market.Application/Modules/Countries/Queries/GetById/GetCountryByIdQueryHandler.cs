using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Countries.Queries.GetById
{
    public sealed class GetCountryByIdQueryHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<GetCountryByIdQuery,GetCountryByIdQueryDto>
    {
        public async Task<GetCountryByIdQueryDto> Handle(GetCountryByIdQuery request, CancellationToken ct)
        {
            var cou = await context.Countries
                .AsNoTracking()
                .Where(c => c.Id == request.Id)
                .Select(c => new GetCountryByIdQueryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Cities = c.Cities.OrderBy(cc => cc.Name).Select(cc => cc.Name).ToList()
                })
                .FirstOrDefaultAsync(ct);

            if (cou is null)
                throw new MarketNotFoundException("Country not found.");

            return cou;



        }
    }
}
