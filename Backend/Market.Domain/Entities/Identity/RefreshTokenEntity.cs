// RefreshTokenEntity.cs

// RefreshTokenEntity.cs
using Market.Domain.Common;
using Market.Domain.Common.Attributes;

namespace Market.Domain.Entities.Identity;


public sealed class RefreshTokenEntity : BaseEntity
{
    [NoAudit]
    [PreserveString]
    public string TokenHash { get; set; } // Store the HASH, not the plain token
    public DateTime ExpiresAtUtc { get; set; }
    public bool IsRevoked { get; set; }
    public int UserId { get; set; }
    public UserEntity User { get; set; } = default!;
    public DateTime? RevokedAtUtc { get; set; }
}