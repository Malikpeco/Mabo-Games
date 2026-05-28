namespace Market.Application.Modules.UserNotifications.Commands.Broadcast
{
    public sealed class NotifyAllUsersCommandValidator : AbstractValidator<NotifyAllUsersCommand>
    {
        public NotifyAllUsersCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}