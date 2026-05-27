namespace Market.Application.Modules.UserNotifications.Queries.List
{
    public sealed class ListUserNotificationsQueryHandler : IRequestHandler<ListUserNotificationsQuery, PageResult<ListUserNotificationsQueryDto>>
    {
        public async Task<PageResult<ListUserNotificationsQueryDto>> Handle(ListUserNotificationsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
