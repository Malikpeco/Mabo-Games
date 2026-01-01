using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Countries.Commands.Update
{
    public sealed class UpdateCountryCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
        :IRequestHandler<UpdateCountryCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateCountryCommand request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this.");

            var cou = await context.Countries.FirstOrDefaultAsync(c=>c.Id == request.Id, ct);
            if (cou is null)
                throw new MarketNotFoundException("Country not found.");

            if (await context.Countries.AnyAsync(c => c.Name.ToLower() == request.Name.ToLower(), ct))
                throw new Exception("Country already exists.");

            cou.Name= request.Name;
            await context.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
