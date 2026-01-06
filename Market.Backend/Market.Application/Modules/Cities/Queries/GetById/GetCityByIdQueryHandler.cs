using Market.Application.Modules.Cities.Queries.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Cities.Queries.GetById
{
    public sealed class GetCityByIdQueryHandler(IAppDbContext context, IAppCurrentUser currentUser)
        :IRequestHandler<GetCityByIdQuery, GetCityByIdQueryDto>
    {
        public async Task<GetCityByIdQueryDto> Handle(GetCityByIdQuery request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this.");

            var result = await context.Cities
                .AsNoTracking()
                .Where(c => c.Id == request.Id)
                .Select(c => new GetCityByIdQueryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    CountryId = c.CountryId,
                    Country = new CountryDto
                    {
                        Id = c.Country.Id,
                        Name = c.Country.Name
                    },
                    Longitude = c.Longitude,
                    Latitude = c.Latitude,
                    PostalCode = c.PostalCode,
                    Users = c.Users.Select(u => new UserDto
                    {
                        Id = u.Id,
                        Username = u.Username
                    }).ToList()
                }).FirstOrDefaultAsync(ct);


            if (result is null)
                throw new MarketNotFoundException("City not found.");

            return result;

        }
    }

}
