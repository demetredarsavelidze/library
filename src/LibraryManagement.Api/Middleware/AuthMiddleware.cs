namespace LibraryManagement.Api.Middleware;

public sealed class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthMiddleware> _logger;

    public AuthMiddleware(RequestDelegate next, ILogger<AuthMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
        {
            _logger.LogWarning("Unauthorized request to {Path}", context.Request.Path);
        }
        else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
        {
            _logger.LogWarning("Forbidden request to {Path}", context.Request.Path);
        }
    }
}
