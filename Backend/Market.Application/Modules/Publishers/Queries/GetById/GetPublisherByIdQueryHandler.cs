using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Publishers.Queries.GetById
{
    public sealed class GetPublisherByIdQueryHandle(IAppCurrentUser currentUser, IAppDbContext context)
        :IRequestHandler<GetPublisherByIdQuery, GetPublisherByIdQueryDto>
    {
        public async Task<GetPublisherByIdQueryDto> Handle(GetPublisherByIdQuery request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this.");

            var pub = await context.Publishers
                .AsNoTracking()
                .Where(p => p.Id == request.Id)
                .Select(p => new GetPublisherByIdQueryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Country = new Queries.List.PublisherCountryDto
                    {
                        Id = p.Country.Id,
                        Name = p.Country.Name,
                    },
                    Games = p.Games.Select(g => new Queries.List.PublishersGameDto
                    {
                        Id = g.Id,
                        Name = g.Name,
                        Description = g.Description,
                        ReleaseDate = g.ReleaseDate,
                    }).ToList()
                }).FirstOrDefaultAsync(ct);

            if (pub is null)
                throw new MarketNotFoundException("Publisher not found.");

            return pub;


        }
    }
}
