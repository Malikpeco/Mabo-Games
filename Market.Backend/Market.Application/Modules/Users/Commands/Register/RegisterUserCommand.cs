using Market.Application.Modules.Users.Dto;
using Market.Domain.Common.Attributes;

namespace Market.Application.Modules.Users.Commands.Register
{
    public sealed class RegisterUserCommand : IRequest<RegisterUserResultDto>
    {
        
        required public string FirstName {  get; set; }

        
        required public string LastName { get; set; }

        
        required public string Username {  get; set; }

        required public string Email { get; set; }

        [PreserveString]
        required public string Password { get; set; }

        [PreserveString]
        required public string ConfirmPassword {  get; set; }

        [PreserveString]
        public string? PhoneNumber { get; set; }

        public int CountryId { get; set; }


    }
}
