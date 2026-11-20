using System.Text.Json;
using Erp.SharedKernel.Responses;
using Microsoft.AspNetCore.Diagnostics;

namespace Erp.Api.Middleware;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly IHostEnvironment _env;

    public GlobalExceptionHandler(IHostEnvironment env)
    {
        _env = env;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (httpContext.Request.Path.StartsWithSegments("/hubs"))
            return false;

        var traceId = httpContext.TraceIdentifier;

        var (statusCode, error) = exception switch
        {
            BadHttpRequestException => (
                StatusCodes.Status400BadRequest,
                ErrorResponse.BadRequest("The submitted data format is invalid.", code: "INVALID_DATA_FORMAT", traceId: traceId)),
            ArgumentException ex => (
                StatusCodes.Status400BadRequest,
                ErrorResponse.BadRequest(ex.Message, code: "VALIDATION_ERROR", traceId: traceId)),
            InvalidOperationException ex => (
                StatusCodes.Status409Conflict,
                ErrorResponse.Conflict(ex.Message, code: "INVALID_STATUS_TRANSITION", traceId: traceId)),
            KeyNotFoundException ex => (
                StatusCodes.Status404NotFound,
                ErrorResponse.NotFound(ex.Message, code: "RESOURCE_NOT_FOUND", traceId: traceId)),
            UnauthorizedAccessException ex => (
                StatusCodes.Status401Unauthorized,
                ErrorResponse.Unauthorized(ex.Message, code: "SESSION_EXPIRED", traceId: traceId)),
            _ => (
                StatusCodes.Status500InternalServerError,
                ErrorResponse.InternalError(
                    _env.IsDevelopment() ? exception.Message : "Something went wrong on our end. Please try again later.",
                    code: "INTERNAL_ERROR",
                    traceId: traceId))
        };

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(error, JsonOptions, cancellationToken);
        return true;
    }
}
