using Market.Domain.Common;

namespace Market.Domain.Entities
{
    public class GameEntity : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int PublisherId { get; set; }
        public PublisherEntity Publisher { get; set; }
        public string? CoverImageURL { get; set; }
        public IReadOnlyCollection<UserGameEntity> UserGames { get; private set; } = new List<UserGameEntity>();
        public IReadOnlyCollection<FavouriteEntity> Favourites { get; private set; } = new List<FavouriteEntity>();
        public IReadOnlyCollection<CartItemEntity> CartItems { get; private set; } = new List<CartItemEntity>();
        public IReadOnlyCollection<GameGenreEntity> GameGenres { get; private set; } = new List<GameGenreEntity>();
        public IReadOnlyCollection<OrderItemEntity> OrderItems { get; private set; } = new List<OrderItemEntity>();
        public IReadOnlyCollection<ScreenshotEntity> Screenshots { get; private set; } = new List<ScreenshotEntity>();



    }
}
