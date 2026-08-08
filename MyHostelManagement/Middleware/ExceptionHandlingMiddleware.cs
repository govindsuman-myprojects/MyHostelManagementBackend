using MyHostelManagement.Services;
using System.Net;
using System.Text.Json;

namespace MyHostelManagement.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApiException ex)
            {
                await WriteResponse(context, ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception processing {Method} {Path}",
                    context.Request.Method, context.Request.Path);
                await WriteResponse(context, (int)HttpStatusCode.InternalServerError,
                    "An unexpected error occurred.");
            }
        }

        private static Task WriteResponse(HttpContext context, int statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                success = false,
                message
            }));
        }
    }
}
