using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Commands.ChangeBio
{
    public sealed class ChangeProfileBioCommandHandler(IAppDbContext context, IAppCurrentUser appCurrentUser) : IRequestHandler<ChangeProfileBioCommand, Unit>
    {
        public async Task<Unit> Handle(ChangeProfileBioCommand request, CancellationToken cancellationToken)
        {
            if (!appCurrentUser.IsAuthenticated)
                throw new MarketForbiddenException();

            var user = await context.Users.FirstAsync(x => x.Id == appCurrentUser.UserId, cancellationToken);


            string? oldBio=user.ProfileBio;


            if (oldBio ==request.NewBio)
                return Unit.Value;


            user.ProfileBio = request.NewBio;


            await context.SaveChangesAsync(cancellationToken);



            return Unit.Value;
        }
    }
}
