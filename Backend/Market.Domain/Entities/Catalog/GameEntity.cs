using Market.Domain.Common;
using Market.Domain.Common.Attributes;

namespace Market.Domain.Entities
{
    public class GameEntity : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        [PreserveString]
        public string? Description { get; set; }

        public DateTime ReleaseDate { get; set; }
        public int PublisherId { get; set; }
        public PublisherEntity Publisher { get; set; }

        [PreserveString]
        public string? CoverImageURL { get; set; }


        public IReadOnlyCollection<UserGameEntity> UserGames { get; internal set; } = new List<UserGameEntity>();
        public IReadOnlyCollection<FavouriteEntity> Favourites { get; internal set; } = new List<FavouriteEntity>();
        public IReadOnlyCollection<CartItemEntity> CartItems { get; internal set; } = new List<CartItemEntity>();
        public IReadOnlyCollection<GameGenreEntity> GameGenres { get; internal set; } = new List<GameGenreEntity>();
        public IReadOnlyCollection<OrderItemEntity> OrderItems { get; internal set; } = new List<OrderItemEntity>();
        public IReadOnlyCollection<ScreenshotEntity> Screenshots { get; internal set; } = new List<ScreenshotEntity>();

        public void AddGenre(int genreId)
        {
            ((List<GameGenreEntity>)GameGenres).Add(new GameGenreEntity { GenreId = genreId });
        }

        public void AddScreenshot(string url)
        {
            ((List<ScreenshotEntity>)Screenshots).Add(new ScreenshotEntity { ImageURL = url });
        }
    }
}