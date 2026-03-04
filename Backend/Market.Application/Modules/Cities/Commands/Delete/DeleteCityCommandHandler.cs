using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Cities.Commands.Delete
{
    public sealed class DeleteCityCommandHandler(IAppCurrentUser currentUser, IAppDbContext context)
        :IRequestHandler<DeleteCityCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteCityCommand request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
            {
                throw new Exception("You must be an admin to do this.");
            }
            
            var city = await context.Cities.FirstOrDefaultAsync(c => c.Id == request.Id, ct);
            if (city is null)
                throw new MarketNotFoundException("City not found.");

            context.Cities.Remove(city);
            await context.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
