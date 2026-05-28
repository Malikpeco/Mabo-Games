namespace Market.Application.Modules.UserNotifications.Queries.GetById
{
    public sealed class GetUserNotificationsByIdQueryDto
    {
        public int Id { get; set; }
        public required string Title { get; init; }
        public required string Content { get; init; }
        public bool IsRead { get; init; }
        public DateTime SentAt { get; init; }
    }
}
