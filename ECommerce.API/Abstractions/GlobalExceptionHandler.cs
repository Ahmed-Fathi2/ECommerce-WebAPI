using ECommerce.Application.Common.Errors;
using ECommerce.Application.Contracts;
using ECommerce.Application.Common.Settings;
//using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Abstractions
{
    public class GlobalExceptionHandler : Microsoft.AspNetCore.Diagnostics.IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            // Log the exception with the request path for better diagnostics
            _logger.LogError(exception, "An unhandled exception occurred. Path: {Path}", httpContext.Request.Path); 

            var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
            };

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(problemDetails);

            return true;
        }
    }
}




