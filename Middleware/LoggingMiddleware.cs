using System.Text;
using Microsoft.IO;

namespace EventMngt.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;
    private readonly RecyclableMemoryStreamManager _streamManager;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _streamManager = new RecyclableMemoryStreamManager();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log request
        var requestBody = await LogRequest(context.Request);

        // Log response
        var originalBodyStream = context.Response.Body;
        using var responseBody = _streamManager.GetStream();
        context.Response.Body = responseBody;

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the request");
            throw;
        }
        finally
        {
            var response = await LogResponse(context.Response);
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    private async Task<string> LogRequest(HttpRequest request)
    {
        request.EnableBuffering();
        var requestBody = string.Empty;

        try
        {
            if (request.Body.Length > 0)
            {
                // Limit the request body size to 1MB to prevent memory issues
                if (request.Body.Length > 1024 * 1024)
                {
                    _logger.LogWarning("Request body exceeds 1MB limit. Truncating log.");
                    requestBody = "[Request body too large to log]";
                }
                else
                {
                    using var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true);
                    requestBody = await reader.ReadToEndAsync();
                    request.Body.Position = 0;
                }
            }

            _logger.LogInformation(
                "HTTP {RequestMethod} {RequestPath} {RequestBody}",
                request.Method,
                request.Path,
                requestBody);

            return requestBody;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging request body");
            return "[Error reading request body]";
        }
    }

    private async Task<string> LogResponse(HttpResponse response)
    {
        var responseBody = string.Empty;
        try
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            
            // Limit the response body size to 1MB
            if (response.Body.Length > 1024 * 1024)
            {
                _logger.LogWarning("Response body exceeds 1MB limit. Truncating log.");
                responseBody = "[Response body too large to log]";
            }
            else
            {
                using var reader = new StreamReader(response.Body, Encoding.UTF8, true, 1024, true);
                responseBody = await reader.ReadToEndAsync();
                response.Body.Seek(0, SeekOrigin.Begin);
            }

            _logger.LogInformation(
                "HTTP {StatusCode} {ResponseBody}",
                response.StatusCode,
                responseBody);

            return responseBody;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging response body");
            return "[Error reading response body]";
        }
    }
} 