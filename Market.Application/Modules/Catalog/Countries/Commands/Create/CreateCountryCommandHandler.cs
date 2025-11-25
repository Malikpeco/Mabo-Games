using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Countries.Commands.Create
{
    public class CreateCountryCommandHandler(IAppDbContext context)
    : IRequestHandler<CreateCountryCommand, int>
    {
        public async Task<int> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            bool exists = await context.Countries.AnyAsync(x => x.Name == request.Name, cancellationToken);

            if (exists)
            {
                throw new MarketConflictException("Country already exists");
            }

            var country = new CountryEntity()
            {
                Name = request.Name
            };

            context.Countries.Add(country);
            await context.SaveChangesAsync(cancellationToken);
            return country.Id;


        }
    }
}
