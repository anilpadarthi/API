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
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var routeData = context.GetRouteData();

                var controller = routeData?.Values["controller"]?.ToString();
                var action = routeData?.Values["action"]?.ToString();
                var httpMethod = context.Request.Method;
                var path = context.Request.Path;
                var queryString = context.Request.QueryString.ToString();
                var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var requestBody = await ReadRequestBody(context);

                var logDetails = new
                {
                    Controller = controller,
                    Action = action,
                    HttpMethod = httpMethod,
                    Path = path,
                    QueryString = queryString,
                    UserId = userId,
                    RequestBody = requestBody
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

        private async Task<string> ReadRequestBody(HttpContext context)
        {
            context.Request.EnableBuffering();

            using var reader = new StreamReader(
                context.Request.Body,
                Encoding.UTF8,
                leaveOpen: true);

            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            return body;
        }
    }


}
