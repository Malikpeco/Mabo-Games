namespace Market.Application.Modules.Users.Commands.UpdateCurrentUserProfile
{
    public sealed class UpdateCurrentUserProfileCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<UpdateCurrentUserProfileCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateCurrentUserProfileCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.IsAuthenticated || !currentUser.UserId.HasValue)
            {
                throw new MarketForbiddenException();
            }

            var user = await context.Users.FirstAsync(x => x.Id == currentUser.UserId.Value, cancellationToken);

            var normalizedUsername = request.Username.Trim();
            var normalizedBio = string.IsNullOrWhiteSpace(request.Bio) ? null : request.Bio.Trim();

            if (!string.Equals(user.Username, normalizedUsername, StringComparison.Ordinal))
            {
                var usernameExists = await context.Users.AnyAsync(
                    x => x.Username == normalizedUsername && x.Id != user.Id,
                    cancellationToken);

                if (usernameExists)
                {
                    throw new MarketConflictException("Username already exists, try again.");
                }

                user.Username = normalizedUsername;
            }

            if (!string.Equals(user.ProfileBio, normalizedBio, StringComparison.Ordinal))
            {
                user.ProfileBio = normalizedBio;
            }

            if (user.CountryId != request.CountryId)
            {
                if (request.CountryId.HasValue)
                {
                    var countryExists = await context.Countries.AnyAsync(
                        x => x.Id == request.CountryId.Value,
                        cancellationToken);

                    if (!countryExists)
                    {
                        throw new MarketNotFoundException("Country not found.");
                    }
                }

                user.CountryId = request.CountryId;
            }

            

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}