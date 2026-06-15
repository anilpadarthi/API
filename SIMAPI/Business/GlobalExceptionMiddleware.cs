using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace SIMAPI.Business
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ErrorLogService logger)
        {
            string requestBody = string.Empty;

            try
            {
                // Only attempt to read body for methods that usually contain one
                if (context.Request.ContentLength > 0)
                {
                    try
                    {
                        context.Request.EnableBuffering();

                        using var reader = new StreamReader(
                            context.Request.Body,
                            Encoding.UTF8,
                            leaveOpen: true);

                        requestBody = await reader.ReadToEndAsync();
                        context.Request.Body.Position = 0;
                    }
                    catch
                    {
                        requestBody = "[Unable to read request body]";
                    }
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                // Client disconnected - don't log as application error
                if (context.RequestAborted.IsCancellationRequested)
                    return;

                var routeData = context.GetRouteData();

                var logDetails = new
                {
                    Controller = routeData?.Values["controller"]?.ToString(),
                    Action = routeData?.Values["action"]?.ToString(),
                    HttpMethod = context.Request.Method,
                    Path = context.Request.Path.ToString(),
                    UserId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    RequestBody = requestBody
                };

                await logger.LogErrorAsync(ex, JsonSerializer.Serialize(logDetails));

                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        status = false,
                        message = "An unexpected error occurred."
                    }));
                }
            }
        }

    }


}
