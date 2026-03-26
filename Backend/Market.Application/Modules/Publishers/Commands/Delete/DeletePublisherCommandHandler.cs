using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Publishers.Commands.Delete
{
    public sealed class DeletePublisherCommandHandler(IAppCurrentUser currentUser, IAppDbContext context)
        : IRequestHandler<DeletePublisherCommand, Unit>
    {
        public async Task<Unit> Handle(DeletePublisherCommand request, CancellationToken ct)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this");

            var pub = await context.Publishers.FirstOrDefaultAsync(p => p.Id == request.Id, ct);
            
            if (pub is null)
                throw new MarketNotFoundException("Publisher not found");

            if (await context.Games.AnyAsync(g => g.PublisherId == request.Id, ct))
                throw new MarketBusinessRuleException("400", "Cannot delete publisher because it has games assigned.");

            context.Publishers.Remove(pub);
            await context.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}
