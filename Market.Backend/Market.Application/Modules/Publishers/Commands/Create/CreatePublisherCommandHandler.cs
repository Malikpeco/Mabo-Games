using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Publishers.Commands.Create
{
    public sealed class CreatePublisherCommandHandler(IAppCurrentUser currentUser, IAppDbContext context)
        :IRequestHandler<CreatePublisherCommand, int>
    {
        public async Task<int> Handle(CreatePublisherCommand request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this.");

            if (!await context.Countries.AnyAsync(c => c.Id == request.CountryId, ct))
                throw new MarketNotFoundException("Country not found.");

            var pub = new PublisherEntity
            {
                Name = request.Name,
                CountryId = request.CountryId,
            };
            context.Publishers.Add(pub);
            await context.SaveChangesAsync(ct);

            return pub.Id;

        }
    }
}
