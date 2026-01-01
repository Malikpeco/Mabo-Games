using Market.Domain.Entities;

namespace Market.Application.Modules.Countries.Commands.Create
{
    public sealed class CreateCountryCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
    : IRequestHandler<CreateCountryCommand, Unit>
    {
        public async Task<Unit> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.IsAdmin)
                throw new Exception("You must be an admin to do this!");

            bool exists = await context.Countries.AnyAsync(x => x.Name.ToLower() == request.Name.ToLower(), cancellationToken);

            if (exists)
            {
                throw new MarketConflictException("Country already exists");
            }

            var country = new CountryEntity()
            {
                Name = request.Name.Trim()
            };

            context.Countries.Add(country);
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;


        }
    }
}
