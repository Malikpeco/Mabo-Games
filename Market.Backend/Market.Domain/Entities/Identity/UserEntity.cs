// MarketUserEntity.cs

// MarketUserEntity.cs
using Market.Domain.Common;
using Market.Domain.Common.Attributes;

namespace Market.Domain.Entities.Identity;

public sealed class UserEntity : BaseEntity
{
    
    public string Username { get; set; }

    [NoAudit]
    [PreserveString]
    public string PasswordHash { get; set; }

    [NoAudit]
    public string Email { get; set; }

    [NoAudit]
    [PreserveString]
    public string? PhoneNumber { get; set; }

    
    public string FirstName { get; set; }

    
    public string LastName { get; set; }

    [PreserveString]
    public string? ProfileImageURL { get; set; }

    [PreserveString]
    public string? ProfileBio { get; set; }
    public int? CityId { get; set; }
    public CityEntity? City { get; set; }
    public int? CountryId {  get; set; }
    public CountryEntity? Country { get; set; }
    public DateTime CreationDate { get; set; }
    public bool IsAdmin { get; set; }
    public CartEntity Cart { get; set; }
    public IReadOnlyCollection<UserGameEntity> UserGames { get; private set; } = new List<UserGameEntity>();
    public IReadOnlyCollection<UserAchievementEntity> UserAchievements { get; private set; } = new List<UserAchievementEntity>();
    public IReadOnlyCollection<FavouriteEntity> Favourites { get; private set; } = new List<FavouriteEntity>();
    public IReadOnlyCollection<NotificationEntity> Notifications { get; private set; } = new List<NotificationEntity>();
    public IReadOnlyCollection<OrderEntity> Orders { get; private set; } = new List<OrderEntity>();
    public IReadOnlyCollection<UserSecurityQuestionEntity> UserSecurityQuestions { get; private set; } = new List<UserSecurityQuestionEntity>();

    public int TokenVersion { get; set; } = 0;// For global revocation
    public bool IsEnabled { get; set; }
    public ICollection<RefreshTokenEntity> RefreshTokens { get; private set; } = new List<RefreshTokenEntity>();



}