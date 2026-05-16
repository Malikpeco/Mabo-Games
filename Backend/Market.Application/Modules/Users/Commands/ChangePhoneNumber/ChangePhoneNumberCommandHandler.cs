namespace Market.Application.Modules.Users.Commands.ChangePhoneNumber;

public sealed class ChangePhoneNumberCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
    : IRequestHandler<ChangePhoneNumberCommand, Unit>
{
    public async Task<Unit> Handle(ChangePhoneNumberCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || !currentUser.UserId.HasValue)
        {
            throw new MarketForbiddenException();
        }

        var user = await context.Users.FirstAsync(x => x.Id == currentUser.UserId.Value, cancellationToken);

        var normalizedPhoneNumber = request.PhoneNumber.Trim();

        if (!string.Equals(user.PhoneNumber, normalizedPhoneNumber, StringComparison.Ordinal))
        {
            user.PhoneNumber = normalizedPhoneNumber;
            await context.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}
