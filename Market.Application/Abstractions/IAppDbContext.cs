namespace Market.Application.Abstractions;

// Application layer
public interface IAppDbContext
{
    DbSet<AchievementEntity> Achievements { get; }
    DbSet<CartEntity> Carts { get; }
    DbSet<CartItemEntity> CartItems { get; }
    DbSet<CityEntity> Cities { get; }
    DbSet<CountryEntity> Countries { get; }
    DbSet<FavouriteEntity> Favourites { get; }
    DbSet<GameEntity> Games { get; }
    DbSet<GenreEntity> Genres { get; }
    DbSet<GameGenreEntity> GameGenres { get; }
    DbSet<NotificationEntity> Notifications { get; }
    DbSet<OrderEntity> Orders { get; }
    DbSet<OrderItemEntity> OrderItem { get; }
    DbSet<PaymentEntity> Payments { get; }
    DbSet<PublisherEntity> Publishers { get; }
    DbSet<ReviewEntity> Reviews { get; }
    DbSet<ScreenshotEntity> Screenshots { get; }
    DbSet<SecurityQuestionEntity> SecurityQuestions { get; }
    DbSet<UserAchievementEntity> UserAchievements { get; }
    DbSet<UserEntity> Users { get; }
    DbSet<UserGameEntity> UserGames { get; }
    DbSet<UserSecurityQuestionEntity> UserSecurityQuestions { get; }
    DbSet<MarketUserEntity> Users { get; }
    DbSet<RefreshTokenEntity> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken ct);
}