using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Cities.Commands.Update
{
    public sealed class UpdateCityCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
        :IRequestHandler<UpdateCityCommand, int>
    {
        public async Task<int> Handle(UpdateCityCommand request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this.");

            var city = await context.Cities.FirstOrDefaultAsync(c => c.Id == request.Id, ct);
            if (city is null)
                throw new MarketNotFoundException("City not found");

            city.Name= request.Name;
            city.Latitude= request.Latitude;
            city.Longitude= request.Longitude;
            city.CountryId= request.CountryId;
            city.PostalCode= request.PostalCode;

            await context.SaveChangesAsync(ct);
            return city.Id;
        }
    }
}
