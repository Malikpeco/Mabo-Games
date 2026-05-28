namespace Market.Application.Modules.UserNotifications.Queries.List
{
    public sealed class ListUserNotificationsQueryHandler(IAppCurrentUser currentUser,IAppDbContext context)
           : IRequestHandler<ListUserNotificationsQuery, List<ListUserNotificationsQueryDto>>
    {
        public async Task<List<ListUserNotificationsQueryDto>> Handle(ListUserNotificationsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUser.IsAuthenticated)
                throw new MarketForbiddenException();

            var q = context.UserNotifications
                .Where(un => un.UserId == currentUser.UserId);

            if (request.IsRead.HasValue && request.IsRead.Value)
                q = q.Where(un => un.IsRead);

            return await q
                .OrderBy(un => un.IsRead)
                .ThenByDescending(un => un.Notification.SentAt)
                .Select(un => new ListUserNotificationsQueryDto
                {
                    Id = un.Id,
                    Title = un.Notification.Title,
                    Content = un.Notification.Content,
                    IsRead = un.IsRead,
                    SentAt = un.Notification.SentAt,
                })
                .ToListAsync(cancellationToken);
        }
    }

}
