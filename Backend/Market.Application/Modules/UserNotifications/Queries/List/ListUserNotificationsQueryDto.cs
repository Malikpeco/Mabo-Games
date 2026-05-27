namespace Market.Application.Modules.UserNotifications.Queries.List
{
    public sealed class ListUserNotificationsQueryDto
    {
        public int Id { get; init; }
        public required string Title { get; init; }
        public required string Content { get; init; }
        public bool IsRead { get; init; }
        public DateTime SentAt { get; init; }
    }
}
