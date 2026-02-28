using Azure.Core;
using Market.Application.Abstractions;
using Market.Application.Common.Email;
using Market.Application.Common.IGDB;
using Market.Application.Common.Security;
using Market.Application.Modules.IGDB.Queries.GetIGDBGameDetails;
using Market.Application.Modules.IGDB.Queries.SearchIGDBGames;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Market.Infrastructure.Common
{
    public sealed class IGDBService : IIGDBService
    {
        private readonly HttpClient _httpClient;
        private readonly IGDBSettings _settings;
        private readonly IAppDbContext context;



        internal record TokenRequstResponse(string access_token, int expires_in);


        public IGDBService(HttpClient httpClient, IOptions<IGDBSettings> options, IAppDbContext dbContext)
        {
            _httpClient = httpClient;
            _settings = options.Value;
            context = dbContext;
        }



        public async Task<string> getTokenAsync(CancellationToken ct)
        {
            var token = await context.IGDBTokens.FirstOrDefaultAsync(ct);

            if (token != null && token.ExpiresAt > DateTime.UtcNow)
                return token.Token;


            var response = await _httpClient.PostAsync(
                $"{_settings.TokenUrl}?client_id={_settings.ClientID}&client_secret={_settings.ClientSecret}&grant_type=client_credentials",
                null, ct);

            var tokenRequestResponse = await response.Content.ReadFromJsonAsync<TokenRequstResponse>(ct);

            response.EnsureSuccessStatusCode();

            if (token == null)
            {
                token = new IGDBTokenEntity();
                context.IGDBTokens.Add(token);
            }

            token.Token = tokenRequestResponse.access_token;
            token.ExpiresAt = DateTime.UtcNow.AddSeconds(tokenRequestResponse.expires_in);
            token.LastUpdated = DateTime.UtcNow;

            await context.SaveChangesAsync(ct);

            return token.Token;

        }

        
        public async Task<List<SearchIGDBGamesQueryDto>> SearchGamesAsync(string searchTerm, CancellationToken ct)
        {
            var token = await getTokenAsync(ct);

            var request = new HttpRequestMessage(HttpMethod.Post, _settings.BaseUrl + "/games")
            {
                Headers = {
                    Authorization = new AuthenticationHeaderValue("Bearer", token)
                },
                Content = new StringContent(
                    $"search \"{searchTerm}\"; " +
                    $"fields id,name,cover.url; " +
                    $"limit 10;",
                    Encoding.UTF8,
                    "text/plain")
            };
            request.Headers.Add("Client-ID", _settings.ClientID);

            var response = await _httpClient.SendAsync(request, ct);
            response.EnsureSuccessStatusCode();

            var igdbGames = await response.Content.ReadFromJsonAsync<List<IGDBSearchResponse>>(ct);

            return igdbGames?.Select(game => new SearchIGDBGamesQueryDto
            {
                Id = game.Id,
                Name = game.Name,
                CoverUrl = FixImageUrl(game.Cover?.Url, "t_cover_small")
            }).ToList() ?? new();
        }

        public async Task<GetIGDBGameDetailsDto> GetGameDetailsAsync(int gameId, CancellationToken ct)
        {
            var token = await getTokenAsync(ct);

            var request = new HttpRequestMessage(HttpMethod.Post, _settings.BaseUrl + "/games")
            {
                Headers = {
                    Authorization = new AuthenticationHeaderValue("Bearer", token)
                },
                Content = new StringContent(
                    $"fields id,name,summary,cover.url,screenshots.url," +
                    $"genres.name,release_dates.date," +
                    $"involved_companies.company.name,involved_companies.developer,involved_companies.publisher; " +
                    $"where id = {gameId};",
                    Encoding.UTF8,
                    "text/plain")
            };

            request.Headers.Add("Client-ID", _settings.ClientID);

            var response = await _httpClient.SendAsync(request, ct);
            response.EnsureSuccessStatusCode();

            var igdbGames = await response.Content.ReadFromJsonAsync<List<IGDBDetailsResponse>>(ct);
            var game = igdbGames?.FirstOrDefault();

            if (game == null)
                throw new Exception("Game not found");

            return new GetIGDBGameDetailsDto
            {
                Id = game.Id,
                Name = game.Name,
                Summary = game.Summary,
                ReleaseDate = game.ReleaseDates?.OrderBy(r => r.Date).FirstOrDefault()?.GetDateTime(),
                CoverUrl = FixImageUrl(game.Cover?.Url, "t_cover_big"),
                Screenshots = game.Screenshots?.Select(s => FixImageUrl(s.Url, "t_screenshot_big")!).ToList() ?? new(),
                Genres = game.Genres?.Select(g => g.Name).ToList() ?? new(),
                Publisher = game.InvolvedCompanies?.FirstOrDefault(ic => ic.Publisher)?.Company?.Name
            };
        }



        private string? FixImageUrl(string? url, string size = "t_cover_big")
        {
            if (string.IsNullOrEmpty(url)) return null;
            return $"https:{url.Replace("t_thumb", size)}";
        }

        // These are all extra classes to extract the nested objects in the response
        private class IGDBSearchResponse
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; } = string.Empty;

            [JsonPropertyName("cover")]
            public CoverDto? Cover { get; set; }
        }


        private class IGDBDetailsResponse
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; } = string.Empty;

            [JsonPropertyName("summary")]
            public string? Summary { get; set; }

            [JsonPropertyName("cover")]
            public CoverDto? Cover { get; set; }

            [JsonPropertyName("screenshots")]
            public List<ScreenshotDto>? Screenshots { get; set; }

            [JsonPropertyName("genres")]
            public List<GenreDto>? Genres { get; set; }

            [JsonPropertyName("release_dates")]
            public List<ReleaseDateDto>? ReleaseDates { get; set; }

            [JsonPropertyName("involved_companies")]
            public List<InvolvedCompanyDto>? InvolvedCompanies { get; set; }
        }

        private class CoverDto
        {
            [JsonPropertyName("url")]
            public string Url { get; set; } = string.Empty;
        }

        private class ScreenshotDto
        {
            [JsonPropertyName("url")]
            public string Url { get; set; } = string.Empty;
        }

        private class GenreDto
        {
            [JsonPropertyName("name")]
            public string Name { get; set; } = string.Empty;
        }

        private class ReleaseDateDto
        {
            [JsonPropertyName("date")]
            public long Date { get; set; }

            public DateTime GetDateTime() => DateTimeOffset.FromUnixTimeSeconds(Date).DateTime;
        }

        private class InvolvedCompanyDto
        {
            [JsonPropertyName("company")]
            public CompanyDto? Company { get; set; }

            [JsonPropertyName("publisher")]
            public bool Publisher { get; set; }
        }

        private class CompanyDto
        {
            [JsonPropertyName("name")]
            public string Name { get; set; } = string.Empty;
        }


    }
}
