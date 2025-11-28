using Market.Application.Abstractions;
using Market.Domain.Entities;

namespace Market.Infrastructure.Database;

public partial class DatabaseContext : DbContext, IAppDbContext
{
    public DbSet<AchievementEntity> Achievements => Set<AchievementEntity>();
    public DbSet<CartItemEntity> CartItems => Set<CartItemEntity>();
    public DbSet<CartEntity> Carts => Set<CartEntity>();
    public DbSet<CityEntity> Cities => Set<CityEntity>();
    public DbSet<CountryEntity> Countries=> Set<CountryEntity>();
    public DbSet<FavouriteEntity> Favourites => Set<FavouriteEntity>();
    public DbSet<GameEntity> Games => Set<GameEntity>();
    public DbSet<GenreEntity> Genres => Set<GenreEntity>();
    public DbSet<GameGenreEntity> GameGenres=> Set<GameGenreEntity>();
    public DbSet<NotificationEntity> Notifications => Set<NotificationEntity>();
    public DbSet<OrderEntity> Orders=> Set<OrderEntity>();
    public DbSet<OrderItemEntity> OrderItem=> Set<OrderItemEntity>();
    public DbSet<PaymentEntity> Payments=> Set<PaymentEntity>();
    public DbSet<PublisherEntity> Publishers => Set<PublisherEntity>();
    public DbSet<ReviewEntity> Reviews => Set<ReviewEntity>();
    public DbSet<ScreenshotEntity> Screenshots => Set<ScreenshotEntity>();
    public DbSet<SecurityQuestionEntity> SecurityQuestions=> Set<SecurityQuestionEntity>();
    public DbSet<UserAchievementEntity> UserAchievements=> Set<UserAchievementEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<UserGameEntity> UserGames  => Set<UserGameEntity>();
    public DbSet<UserSecurityQuestionEntity> UserSecurityQuestions  => Set<UserSecurityQuestionEntity>();
    public DbSet<RefreshTokenEntity> RefreshTokens => Set<RefreshTokenEntity>();

    private readonly TimeProvider _clock;
    public DatabaseContext(DbContextOptions<DatabaseContext> options, TimeProvider clock) : base(options)
    {
        _clock = clock;
    }
}