namespace Market.Application.Modules.UserNotifications.Queries.GetById
{
    public sealed class GetUserNotificationsByIdQueryHandler : IRequestHandler<GetUserNotificationsByIdQuery, GetUserNotificationsByIdQueryDto>
    {
        public async Task<GetUserNotificationsByIdQueryDto> Handle(GetUserNotificationsByIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
