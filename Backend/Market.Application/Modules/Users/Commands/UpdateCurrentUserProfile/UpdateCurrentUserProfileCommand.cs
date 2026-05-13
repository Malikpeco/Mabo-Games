namespace Market.Application.Modules.Users.Commands.UpdateCurrentUserProfile
{
    public sealed class UpdateCurrentUserProfileCommand : IRequest<Unit>
    {
        public string Username { get; set; }

        public string? Bio { get; set; }

        public int? CountryId { get; set; }
    }
}