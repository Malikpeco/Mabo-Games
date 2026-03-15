using Market.Domain.Common.Attributes;

namespace Market.Application.Modules.Auth.Commands.Logout;

/// <summary>
/// Command for user logout and revocation of the refresh token.
/// </summary>
/// 
[PreserveString]
public sealed class LogoutCommand : IRequest
{
    /// <summary>
    /// The refresh token to be revoked.
    /// </summary>
    public string RefreshToken { get; init; }
}