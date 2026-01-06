using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Cities.Commands.Create
{
    public sealed class CreateCityCommandHandler(IAppDbContext context, IAppCurrentUser currentUser) 
        : IRequestHandler<CreateCityCommand, int> 
    {
        public async Task<int> Handle(CreateCityCommand request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this.");

            if (!await context.Countries.AnyAsync(c => c.Id == request.CountryId))
                throw new Exception("CountryId not found.");

            if (await context.Cities.AnyAsync(c => c.Name.ToLower() == request.Name.ToLower().Trim() && c.CountryId == request.CountryId, ct))
                throw new Exception("City already exists in this country.");

            var city = new CityEntity
            {
                Name = request.Name,
                CountryId = request.CountryId,
                Longitude = request.Longitude,
                Latitude = request.Latitude,
                PostalCode = request.PostalCode,
            };
            context.Cities.Add(city);
            await context.SaveChangesAsync(ct);
            return city.Id;
        }
    }
}
