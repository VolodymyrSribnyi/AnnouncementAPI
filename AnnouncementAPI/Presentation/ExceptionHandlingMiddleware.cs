using AnnouncementAPI._01_Domain.Exceptions;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace AnnouncementAPI.Presentation
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var (statusCode, message, logLevel) = GetErrorDetails(ex);

            switch (logLevel)
            {
                case LogLevel.Warning:
                    _logger.LogWarning(ex, message);
                    break;
                case LogLevel.Error:
                    _logger.LogError(ex, message);
                    break;
                case LogLevel.Critical:
                    _logger.LogCritical(ex, message);
                    break;
                default:
                    _logger.LogError(ex, message);
                    break;
            }

            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = statusCode;

                await HandleError(context, ex, statusCode, message);
            }
        }

        private async Task HandleError(HttpContext context, Exception ex, int statusCode, string? message)
        {
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                StatusCode = statusCode,
                Error = message
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }

        private (int statusCode, string? message, LogLevel logLevel) GetErrorDetails(Exception exception)
        {
            return exception switch
            {
                NotFoundException ex => (StatusCodes.Status404NotFound, ex.Message, LogLevel.Warning),

                BadRequestException ex => (StatusCodes.Status400BadRequest, ex.Message, LogLevel.Error),

                ArgumentNullException ex => (StatusCodes.Status400BadRequest,
                "Required parameter is missing", LogLevel.Error),

                ArgumentException ex => (StatusCodes.Status400BadRequest, ex.Message, LogLevel.Error),

                InvalidOperationException ex => (StatusCodes.Status400BadRequest, ex.Message, LogLevel.Error),

                UnauthorizedAccessException ex => (StatusCodes.Status401Unauthorized, ex.Message, LogLevel.Error),

                ValidationException ex => (StatusCodes.Status400BadRequest,
                ex.Message, LogLevel.Warning),

                TimeoutException => (StatusCodes.Status408RequestTimeout,
                    "Request timeout", LogLevel.Warning),

                _ => (StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred", LogLevel.Critical)
            };
        }
    }
}
