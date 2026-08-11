using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WarehouseERP.Application.Common.Exceptions;
using WarehouseERP.Domain.Exceptions;

namespace WarehouseERP.Api.Middleware;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, title, detail) = MapException(exception);

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception occurred.");
        }

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static (int StatusCode, string Title, string Detail) MapException(Exception exception) => exception switch
    {
        NotFoundException => (StatusCodes.Status404NotFound, "Resource not found", exception.Message),
        DuplicateNameException => (StatusCodes.Status409Conflict, "Conflict", exception.Message),
        DuplicateSkuException => (StatusCodes.Status409Conflict, "Conflict", exception.Message),
        DuplicateCodeException => (StatusCodes.Status409Conflict, "Conflict", exception.Message),
        InactiveCategoryException => (StatusCodes.Status400BadRequest, "Business rule violation", exception.Message),
        InactiveWarehouseException => (StatusCodes.Status400BadRequest, "Business rule violation", exception.Message),
        DomainException => (StatusCodes.Status400BadRequest, "Business rule violation", exception.Message),
        _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred", "An unexpected error occurred. Please try again later.")
    };
}
