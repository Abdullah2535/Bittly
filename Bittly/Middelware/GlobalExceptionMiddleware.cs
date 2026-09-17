using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Bittly.Middelware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Pass the request down the pipeline to the controllers
                await _next(context);
            }
            catch (Exception ex)
            {
                //  For the  system crashes (like Database connectivity issues).
                // Serilog intercepts this ILogger call and saves it to your SQL database.
                _logger.LogError(ex, "A critical system fault occurred during {RequestPath}", context.Request.Path);

                await HandleSystemCrashAsync(context);
            }
        }

        private static async Task HandleSystemCrashAsync(HttpContext context)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                Status = 500,
                Title = "Internal Server Error",
                Detail = "An unexpected fault occurred. Our engineers have been notified."
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}