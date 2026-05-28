namespace Market.Application.Modules.UserNotifications.Commands.Broadcast
{
    public sealed class NotifyAllUsersCommand : IRequest<Unit>
    {
        public string Title { get; init; }
        public string Content { get; init; }
    }
}
