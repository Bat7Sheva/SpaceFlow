using System.Text.Json;
using FluentValidation;
using SpaceFlow.Api.Models;
using SpaceFlow.Api.Shared.Errors;

namespace SpaceFlow.Api.Middleware;

public sealed class GlobalExceptionMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            ValidationException validationException =>
                (StatusCodes.Status400BadRequest, string.Join("; ", validationException.Errors.Select(e => e.ErrorMessage))),
            NotFoundException => (StatusCodes.Status404NotFound, exception.Message),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, exception.Message),
            ForbiddenException => (StatusCodes.Status403Forbidden, exception.Message),
            ConflictException => (StatusCodes.Status409Conflict, exception.Message),
            UnprocessableEntityException => (StatusCodes.Status422UnprocessableEntity, exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        if (statusCode >= StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Unhandled exception for request {Path}", context.Request.Path);
        }
        else
        {
            logger.LogWarning(exception, "Handled exception for request {Path}", context.Request.Path);
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var payload = new ApiResponse<object>
        {
            Success = false,
            Data = null,
            Message = message
        };

        var json = JsonSerializer.Serialize(payload);
        await context.Response.WriteAsync(json);
    }
}