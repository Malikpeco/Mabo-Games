using System.Diagnostics;
using System.Text;

namespace Market.API.Middlewares;

/// <summary>
/// Middleware that logs incoming HTTP requests and outgoing responses,
/// including duration, method, path, and status code.
/// </summary>
public sealed class RequestResponseLoggingMiddleware(
    RequestDelegate next,
    ILogger<RequestResponseLoggingMiddleware> logger)
{
    private const int SlowRequestThresholdMs = 400; // 2 seconds

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var request = context.Request;

        // Read request body (only for POST/PUT)
        string? requestBody = null;
        if (request.Method is "POST" or "PUT")
        {
            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            requestBody = await reader.ReadToEndAsync();
            request.Body.Position = 0;
        }

        // Capture original response stream
        var originalBodyStream = context.Response.Body;
        await using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            // Continue pipeline
            await next(context);
        }
        finally
        {
            stopwatch.Stop();

            // Read response body
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var logMessage = new StringBuilder()
                .AppendLine("HTTP Request/Response Log:")
                .AppendLine($"  Path: {request.Path}")
                .AppendLine($"  Method: {request.Method}")
                .AppendLine($"  Status: {context.Response.StatusCode}")
                .AppendLine($"  Duration: {stopwatch.ElapsedMilliseconds} ms");

            if (!string.IsNullOrWhiteSpace(requestBody))
                logMessage.AppendLine($"  Request Body: {requestBody}");

            if (!string.IsNullOrWhiteSpace(responseText))
                logMessage.AppendLine($"  Response Body: {responseText}");

            var elapsed = stopwatch.ElapsedMilliseconds;
            if (elapsed > SlowRequestThresholdMs)
            {
                logger.LogWarning("[SLOW REQUEST] {Path} took {Elapsed} ms", request.Path, elapsed);
                await File.AppendAllTextAsync("Logs/slow-requests.log",
                    $"{DateTime.UtcNow:u} | {request.Path} | {elapsed} ms{Environment.NewLine}");
            }

            logger.LogInformation("{Log}", logMessage.ToString());

            // Copy the response back to the original stream
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
