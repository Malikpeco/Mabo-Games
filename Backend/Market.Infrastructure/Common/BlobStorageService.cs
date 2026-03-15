using Market.Application.Abstractions;
using Market.Application.Common.IGDB;
using Market.Application.Common.ImageBB;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Market.Infrastructure.Common
{
    public sealed class BlobStorageService : IBlobStorageService
    {

        private readonly HttpClient _httpClient;
        private readonly ImageBBSettings _settings;


        public BlobStorageService(HttpClient httpClient, IOptions<ImageBBSettings> options, IAppDbContext dbContext)
        {
            _httpClient = httpClient;
            _settings = options.Value;
        }




        public async Task<string> UploadImageAsync(Stream imageStream, string fileName, CancellationToken ct = default)
        {
 
            var url = $"https://api.imgbb.com/1/upload?key={_settings.ApiKey}";

            using var content = new MultipartFormDataContent();


            var streamContent = new StreamContent(imageStream);
            content.Add(streamContent, "image", fileName);

            var response = await _httpClient.PostAsync(url, content, ct);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ImgBBResponse>(cancellationToken: ct);

            if (result?.Data?.Url == null)
                throw new Exception("Failed to upload image to ImgBB. Response was empty.");

            return result.Data.Url;
        }

        public async Task<List<string>> UploadImagesAsync(List<Stream> imageStreams, List<string> fileNames, CancellationToken ct = default)
        {
            var urls = new List<string>();

            for (int i = 0; i < imageStreams.Count; i++)
            {
                var uploadedUrl = await UploadImageAsync(imageStreams[i], fileNames[i], ct);
                urls.Add(uploadedUrl);
            }

            return urls;
        }




        private class ImgBBResponse
        {
            public ImgBBData? Data { get; set; }
            public bool Success { get; set; }
            public int Status { get; set; }
        }

        private class ImgBBData
        {
            public string? Id { get; set; }
            public string? Title { get; set; }
            public string? Url { get; set; }
            public string? Display_url { get; set; }
            public string? Delete_url { get; set; }
        }




    }
}
