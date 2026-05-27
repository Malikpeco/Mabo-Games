using Market.Domain.Entities.Catalog;

namespace Market.Application.Modules.UserNotifications.Commands.ClearRead
{
    public sealed class ClearReadUserNotificationsCommandHandler(
        IAppCurrentUser currentUser,
        IAppDbContext context) : IRequestHandler<ClearReadUserNotificationsCommand, Unit>
    {
        public async Task<Unit> Handle(ClearReadUserNotificationsCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.IsAuthenticated)
                throw new MarketForbiddenException();

            var readNotifications = await context.UserNotifications
                .Where(un => un.UserId == currentUser.UserId && un.IsRead)
                .ToListAsync(cancellationToken);

            if (readNotifications.Count == 0)
                return Unit.Value;

            context.UserNotifications.RemoveRange(readNotifications);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}