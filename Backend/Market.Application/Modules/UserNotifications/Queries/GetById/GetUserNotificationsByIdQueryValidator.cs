namespace Market.Application.Modules.UserNotifications.Queries.GetById
{
    public sealed class GetUserNotificationsByIdQueryValidator : AbstractValidator<GetUserNotificationsByIdQuery>
    {
        public GetUserNotificationsByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
