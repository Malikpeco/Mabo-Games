using Market.Domain.Common.Attributes;

namespace Market.Application.Modules.Auth.Commands.Refresh;

/// <summary>
/// Request to rotate the refresh token and issue a new token pair.
/// </summary>
/// 
[PreserveString]
public sealed class RefreshTokenCommand : IRequest<RefreshTokenCommandDto>
{
    /// <summary>
    /// Refresh token that the client sends for rotation.
    /// </summary>
    public string RefreshToken { get; init; }

    /// <summary>
    /// (Optional) Client "fingerprint" / device identifier for device-bound tokens.
    /// </summary>
}