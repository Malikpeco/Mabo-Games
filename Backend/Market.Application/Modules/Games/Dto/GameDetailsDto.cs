using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Games.Dto
{
    public sealed class GameDetailsDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = default!;
        public decimal Price { get; init; }
        public DateTime ReleaseDate { get; init; }
        public string? Description { get; init; }
        public string? CoverImageURL { get; init; }
        public PublisherDto Publisher { get; init; } = default!;
        public IReadOnlyList<GameScreenshotsDto> Screenshots { get; init; } = new List<GameScreenshotsDto>();
        public IReadOnlyList<GameGenreDto> Genres { get; init; } = new List<GameGenreDto>();
        public ReviewSummaryDto Reviews { get; init; } = new();
    }

    public class GameScreenshotsDto
    {
        public string ImageURL { get; set; }
        public int GameId { get; set; }
    }

    public sealed class PublisherDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = default!;
        public int CountryId { get; init; }
        public string? CountryName { get; init; }
    }

    public sealed class GameGenreDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = default!;
    }

    public sealed class ReviewSummaryDto
    {
        public float AverageRating { get; init; }
        public int Count { get; init; }
        public IReadOnlyList<ReviewDto> Items { get; init; } = new List<ReviewDto>();
    }

    public sealed class ReviewDto
    {
        public int Id { get; init; }
        public float Rating { get; init; }
        public string? Content { get; init; }
        public DateTime Date { get; init; }
        public int UserId { get; init; }
        public string? Username { get; init; }
    }
}

