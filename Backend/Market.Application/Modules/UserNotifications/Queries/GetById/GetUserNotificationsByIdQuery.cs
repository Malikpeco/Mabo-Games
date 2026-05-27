namespace Market.Application.Modules.UserNotifications.Queries.GetById
{
    public sealed class GetUserNotificationsByIdQuery : IRequest<GetUserNotificationsByIdQueryDto>
    {
        public int Id { get; init; }

    }
}
