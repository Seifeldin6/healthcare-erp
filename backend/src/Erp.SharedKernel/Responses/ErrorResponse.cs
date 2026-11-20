namespace Erp.SharedKernel.Responses;

public sealed record ErrorResponse(
    bool Success,
    int StatusCode,
    string Code,
    string Message,
    IDictionary<string, string[]>? Errors = null,
    string? TraceId = null)
{
    public static ErrorResponse BadRequest(
        string? message = null,
        string code = "BAD_REQUEST",
        string? traceId = null,
        IDictionary<string, string[]>? errors = null)
        => new(false, 400, code, message ?? "The submitted request is invalid.", errors, traceId);

    public static ErrorResponse Unauthorized(
        string? message = null,
        string code = "UNAUTHORIZED",
        string? traceId = null)
        => new(false, 401, code, message ?? "Your session has expired. Please sign in again.", null, traceId);

    public static ErrorResponse Forbidden(
        string? message = null,
        string code = "FORBIDDEN",
        string? traceId = null)
        => new(false, 403, code, message ?? "You do not have permission to perform this action.", null, traceId);

    public static ErrorResponse NotFound(
        string? message = null,
        string code = "RESOURCE_NOT_FOUND",
        string? traceId = null)
        => new(false, 404, code, message ?? "The requested resource was not found.", null, traceId);

    public static ErrorResponse Conflict(
        string? message = null,
        string code = "CONFLICT",
        string? traceId = null)
        => new(false, 409, code, message ?? "A conflict occurred with the current state of the resource.", null, traceId);

    public static ErrorResponse ValidationError(
        string? message = null,
        IDictionary<string, string[]>? errors = null,
        string code = "VALIDATION_ERROR",
        string? traceId = null)
        => new(false, 422, code, message ?? "One or more validation errors occurred.", errors, traceId);

    public static ErrorResponse InternalError(
        string? message = null,
        string code = "INTERNAL_ERROR",
        string? traceId = null)
        => new(false, 500, code, message ?? "Something went wrong on our end. Please try again later.", null, traceId);
}
