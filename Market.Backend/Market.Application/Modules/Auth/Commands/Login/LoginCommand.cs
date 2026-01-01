using Market.Domain.Common.Attributes;

namespace Market.Application.Modules.Auth.Commands.Login;

/// <summary>
/// Command for user login and issuing an access/refresh token pair.
/// </summary>
public sealed class LoginCommand : IRequest<LoginCommandDto>
{
    /// <summary>
    /// User's email.
    /// </summary>
    public string Email { get; init; }

    /// <summary>
    /// User's password.
    /// </summary>
    /// 
    [PreserveString]
    public string Password { get; init; }

}