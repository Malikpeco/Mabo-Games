using Azure.Core;
using Market.Application.Abstractions;
using Market.Application.Common.Email;
using Market.Application.Common.IGDB;
using Market.Application.Common.Security;
using Market.Application.Modules.IGDB.Queries.SearchIGDBGames;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
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

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            _httpClient.DefaultRequestHeaders.Add("Client-ID", _settings.ClientID);

            var body = $"search \"{searchTerm}\"; fields id,name,cover.url,summary; limit 10;";

            var response = await _httpClient.PostAsync(
                _settings.BaseUrl + "/games",
                new StringContent(body),
                ct);

            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<List<SearchIGDBGamesQueryDto>>(ct);

            return result ?? new();
        }



    }
}
