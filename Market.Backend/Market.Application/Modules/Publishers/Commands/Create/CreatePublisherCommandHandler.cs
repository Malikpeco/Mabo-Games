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
                throw new MarketForbiddenException();

            if (!await context.Countries.AnyAsync(c => c.Id == request.CountryId, ct))
                throw new MarketNotFoundException("Country not found.");


            if (await context.Publishers.AnyAsync(c => c.Name == request.Name, ct))
                throw new MarketConflictException("Publisher with that name already exists! .");



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
