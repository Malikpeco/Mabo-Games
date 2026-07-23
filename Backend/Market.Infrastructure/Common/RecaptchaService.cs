using Market.Application.Abstractions;
using Market.Application.Common.Recaptcha;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Market.Infrastructure.Common
{
    public sealed class RecaptchaService : IRecaptchaService
    {
        private readonly HttpClient _httpClient;
        private readonly RecaptchaSettings _settings;

        public RecaptchaService(HttpClient httpClient, IOptions<RecaptchaSettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;
        }

        public async Task<bool> VerifyAsync(string token, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            var response = await _httpClient.PostAsync(
                $"{_settings.VerifyUrl}?secret={_settings.SecretKey}&response={token}",
                null, ct);

            if (!response.IsSuccessStatusCode)
                return false;

            var result = await response.Content.ReadFromJsonAsync<SiteVerifyResponse>(cancellationToken: ct);

            return result?.Success ?? false;
        }

        private sealed class SiteVerifyResponse
        {
            [JsonPropertyName("success")]
            public bool Success { get; set; }

            [JsonPropertyName("error-codes")]
            public List<string>? ErrorCodes { get; set; }
        }
    }
}
