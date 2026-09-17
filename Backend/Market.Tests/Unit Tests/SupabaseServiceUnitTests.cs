using System.Net;
using Market.Application.Common.Supabase;
using Market.Infrastructure.Common;
using Microsoft.Extensions.Options;

namespace Market.Tests.UnitTests;

public class SupabaseServiceUnitTests
{
    private sealed class RecordingHandler : HttpMessageHandler
    {
        public HttpRequestMessage? LastRequest { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequest = request;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }
    }

    private static SupabaseService CreateService(RecordingHandler handler, out SupabaseSettings settings)
    {
        settings = new SupabaseSettings
        {
            BaseUrl = "https://example.supabase.co",
            BucketName = "game-assets",
            ServiceRoleSecret = "test-secret",
        };

        var httpClient = new HttpClient(handler);
        return new SupabaseService(httpClient, Options.Create(settings));
    }

    [Fact]
    public async Task DeleteFileAsync_ShouldRequestExactUrl_WithNoLeadingSpace()
    {
        var handler = new RecordingHandler();
        var service = CreateService(handler, out var settings);

        await service.DeleteFileAsync("games/test.zip", CancellationToken.None);

        Assert.NotNull(handler.LastRequest);
        Assert.Equal(HttpMethod.Delete, handler.LastRequest!.Method);
        Assert.Equal(
            $"{settings.BaseUrl}/storage/v1/object/{settings.BucketName}/games/test.zip",
            handler.LastRequest.RequestUri!.ToString());
    }
}
