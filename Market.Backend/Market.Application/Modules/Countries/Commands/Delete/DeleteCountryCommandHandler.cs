using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Countries.Commands.Delete
{
    public sealed class DeleteCountryCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
        :IRequestHandler<DeleteCountryCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteCountryCommand request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this.");

            var c = await context.Countries.FirstOrDefaultAsync(c=>c.Id==request.Id, ct);
            if (c is null)
                throw new MarketNotFoundException("Country not found.");


            context.Countries.Remove(c);
            await context.SaveChangesAsync(ct);

            return Unit.Value;

        }
    }
}
