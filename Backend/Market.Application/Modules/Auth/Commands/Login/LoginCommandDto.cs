namespace Market.Application.Modules.Auth.Commands.Login;

/// <summary>
/// Represents a pair of tokens (access + refresh) that the client receives upon login or token refresh.
/// </summary>
public sealed class LoginCommandDto
{
    /// <summary>
    /// JWT access token – used for authorized API calls.
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// Refresh token that the client stores locally and uses to obtain a new access token.
    /// </summary>
    public string RefreshToken { get; set; }

    /// <summary>
    /// Expiration time of the access token in UTC format.
    /// </summary>
    public DateTime ExpiresAtUtc { get; set; }
}