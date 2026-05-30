using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Market.Application.Abstractions;
using Market.Application.Common.Supabase;
using Microsoft.Extensions.Options;

namespace Market.Infrastructure.Common
{
    public sealed class SupabaseService : ISupaBaseService
    {
        private readonly HttpClient _httpClient;
        private readonly SupabaseSettings _settings;

        public SupabaseService(HttpClient httpClient, IOptions<SupabaseSettings> options)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _settings = options?.Value ?? throw new ArgumentNullException(nameof(options));

            _httpClient.DefaultRequestHeaders.Add("apikey", _settings.ServiceRoleSecret);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _settings.ServiceRoleSecret);

        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, CancellationToken ct = default)
        {
            var bucket = _settings.BucketName;
            var objectPath = fileName;
            var url = $"{_settings.BaseUrl}/storage/v1/object/{bucket}/{objectPath}";

            using var content = new MultipartFormDataContent();
            content.Add(new StreamContent(fileStream), "file", fileName);


            using var response = await _httpClient.PostAsync(url, content, ct);
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(ct);
                throw new HttpRequestException($"UploadFileAsync failed. Status={(int)response.StatusCode}. Body={errorBody}");
            }

            return objectPath;
        }

        public async Task DeleteFileAsync(string filePath, CancellationToken ct = default)
        {
            var bucket = _settings.BucketName; 

            var url = $" {_settings.BaseUrl}/storage/v1/object/{bucket}/{filePath}";
            using var response = await _httpClient.DeleteAsync(url, ct); 
            
            
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(ct);
                throw new HttpRequestException($"DeleteObject failed. Status={(int)response.StatusCode}.Body= {errorBody} ");
            }
        }


        public async Task<string> GetSignedUrlAsync(string filePath, int expiresInSeconds = 3600, CancellationToken ct = default)
        {
            var bucket = _settings.BucketName;
            var url = $"{_settings.BaseUrl}/storage/v1/object/sign/{bucket}/{filePath}";


            using var response = await _httpClient.PostAsJsonAsync(url, new { expiresIn = expiresInSeconds }, ct);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(ct);
                throw new HttpRequestException($"GetSignedUrl failed. Status={(int)response.StatusCode}. Body={errorBody}");
            }


            var responseData = await response.Content.ReadFromJsonAsync<SupabaseSignResponse>(cancellationToken: ct);

            var signedUrl = responseData?.SignedUrl ?? responseData?.Url;
            if (string.IsNullOrEmpty(signedUrl))
            {
                throw new InvalidOperationException("Unexpected signed URL response structure.");
            }

            return signedUrl;
        }

        private sealed record SupabaseSignResponse(
            [property: JsonPropertyName("signed_url")] string? SignedUrl,
            [property: JsonPropertyName("url")] string? Url
        );
    }
}