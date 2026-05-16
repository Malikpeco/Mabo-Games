namespace Market.Application.Modules.Users.Commands.DeleteUser;

public sealed class DeleteUserCommandHandler(IAppDbContext context, IAppCurrentUser currentUser, IEmailSender emailSender)
    : IRequestHandler<DeleteUserCommand, Unit>
{
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || !currentUser.UserId.HasValue)
        {
            throw new MarketForbiddenException();
        }

        var user = await context.Users
            .Include(x => x.RefreshTokens)
            .FirstOrDefaultAsync(x => x.Id == currentUser.UserId.Value, cancellationToken);

        if (user is null)
        {
            throw new MarketNotFoundException("User account not found.");
        }

        foreach (var refreshToken in user.RefreshTokens.Where(x => !x.IsRevoked))
        {
            refreshToken.IsRevoked = true;
            refreshToken.RevokedAtUtc = DateTime.UtcNow;
        }

        user.IsEnabled = false;
        user.TokenVersion++;

        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken);

        await emailSender.SendEmail(
            user.Email,
            "Your Mabo account was deleted",
            $"Hi {user.Username},\n\nYour account has been permanently deleted. If this action was not performed by you, please contact support immediately.",
            cancellationToken);

        return Unit.Value;
    }
}
