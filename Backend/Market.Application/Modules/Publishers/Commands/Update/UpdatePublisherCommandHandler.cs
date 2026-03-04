using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Publishers.Commands.Update
{
    public sealed class UpdatePublisherCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
        :IRequestHandler<UpdatePublisherCommand, Unit>
    {
        public async Task<Unit> Handle(UpdatePublisherCommand request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this.");

            var pub = await context.Publishers.FirstOrDefaultAsync(p => p.Id == request.Id, ct);

            if (pub is null)
                throw new MarketNotFoundException("Publisher not found");

            if (!await context.Countries.AnyAsync(c => c.Id == request.CountryId, ct))
                throw new MarketNotFoundException("Country not found");


            pub.Name=request.Name;
            pub.CountryId=request.CountryId;

            await context.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
