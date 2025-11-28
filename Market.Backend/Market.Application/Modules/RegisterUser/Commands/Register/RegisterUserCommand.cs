using Market.Application.Modules.RegisterUser.Dto;

namespace Market.Application.Modules.RegisterUser.Commands.Register
{
    public sealed class RegisterUserCommand : IRequest<RegisterUserResultDto>
    {
        required public string FirstName {  get; set; }

        required public string LastName { get; set; }

        required public string Username {  get; set; }

        required public string Email { get; set; }

        required public string Password { get; set; }

        required public string ConfirmPassword {  get; set; }

        public string? PhoneNumber { get; set; }

        public int CountryId { get; set; }


    }
}
