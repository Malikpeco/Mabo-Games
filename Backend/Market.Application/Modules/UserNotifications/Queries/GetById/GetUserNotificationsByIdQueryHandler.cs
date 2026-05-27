namespace Market.Application.Modules.UserNotifications.Queries.GetById
{
    public sealed class GetUserNotificationsByIdQueryHandler(IAppCurrentUser currentUser, IAppDbContext context)
        : IRequestHandler<GetUserNotificationsByIdQuery, GetUserNotificationsByIdQueryDto>
    {
        public async Task<GetUserNotificationsByIdQueryDto> Handle(GetUserNotificationsByIdQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUser.IsAuthenticated)
                throw new MarketForbiddenException();

            var userNotification = await context.UserNotifications
                .Include(un => un.Notification)
                .Where(un => un.Id == request.Id
                          && un.UserId == currentUser.UserId
                          && !un.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (userNotification is null)
                throw new MarketNotFoundException("Notification not found!");

            if (!userNotification.IsRead)
            {
                userNotification.IsRead = true;
                await context.SaveChangesAsync(cancellationToken);
            }

            return new GetUserNotificationsByIdQueryDto
            {
                Id = userNotification.Id,
                Title = userNotification.Notification.Title,
                Content = userNotification.Notification.Content,
                IsRead = userNotification.IsRead,
                SentAt = userNotification.Notification.SentAt,
            };
        }
    }
}
