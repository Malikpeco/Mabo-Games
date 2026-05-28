using Market.Domain.Entities.Catalog;
using Market.Domain.Entities;

namespace Market.Application.Modules.UserNotifications.Commands.Broadcast
{
    public sealed class NotifyAllUsersCommandHandler(
        IAppCurrentUser currentUser,
        IAppDbContext context) : IRequestHandler<NotifyAllUsersCommand, Unit>
    {
        public async Task<Unit> Handle(NotifyAllUsersCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.IsAdmin)
                throw new MarketForbiddenException();

            var notification = new NotificationEntity
            {
                Title = request.Title,
                Content = request.Content,
            };

            context.Notifications.Add(notification);
            await context.SaveChangesAsync(cancellationToken);

            var userIds = await context.Users
                .Where(u=>!u.IsAdmin)
                .Select(u => u.Id)
                .ToListAsync(cancellationToken);

            var userNotifications = userIds.Select(userId => new UserNotificationsEntity
            {
                UserId = userId,
                NotificationId = notification.Id,
                IsRead = false,
            }).ToList();

            context.UserNotifications.AddRange(userNotifications);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}