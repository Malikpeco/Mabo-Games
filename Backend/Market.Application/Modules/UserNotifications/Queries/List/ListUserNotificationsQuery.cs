namespace Market.Application.Modules.UserNotifications.Queries.List
{
    public sealed class ListUserNotificationsQuery : IRequest<List<ListUserNotificationsQueryDto>>
    {
        public bool? IsRead { get; init; } 
    }
}
