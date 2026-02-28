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
                context.Request.EnableBuffering();

                using (var reader = new StreamReader(
                    context.Request.Body,
                    Encoding.UTF8,
                    leaveOpen: true))
                {
                    requestBody = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0; // VERY IMPORTANT
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                var routeData = context.GetRouteData();

                var logDetails = new
                {
                    Controller = routeData?.Values["controller"]?.ToString(),
                    Action = routeData?.Values["action"]?.ToString(),
                    UserId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    RequestBody = requestBody   // 🔥 Use stored body
                };

                await logger.LogErrorAsync(ex, JsonSerializer.Serialize(logDetails));

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
