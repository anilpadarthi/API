using SIMAPI.Data;
using SIMAPI.Data.Entities;
using System.Net;
using System.Text.Json;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IServiceScopeFactory _scopeFactory;


    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IServiceScopeFactory scopeFactory)
    {
        _next = next;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred.");
            await LogErrorToDatabase(ex); // Store in DB
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task LogErrorToDatabase(Exception ex)
    {
        // Create a new scope to get a fresh DbContext instance
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SIMDBContext>();

            var errorLog = new ErrorInfo
            {
                ErrorMessage = ex.Message,
                StackTrace = ex.StackTrace,
                Method = ex.Source,
                CreatedDate = DateTime.Now
            };

            dbContext.Add(errorLog);
            await dbContext.SaveChangesAsync();

        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var errorResponse = new
        {
            Message = "An unexpected error occurred.",
            Error = exception.Message,
            StatusCode = response.StatusCode
        };

        var json = JsonSerializer.Serialize(errorResponse);
        return response.WriteAsync(json);
    }
}
