using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Proizvodi.Api;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception occurred. TraceId: {TraceId}", httpContext.TraceIdentifier);

        var problemDetails = CreateProblemDetails(exception, httpContext);
        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }

    private static ProblemDetails CreateProblemDetails(Exception exception, HttpContext httpContext)
    {
        var (statusCode, title, detail) = exception switch
        {
            AppException appException => ((int)appException.StatusCode, appException.Message, (string?)appException.Message),
            ArgumentException => (StatusCodes.Status400BadRequest, "Invalid argument provided", (string?)null),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized", (string?)null),
            HttpRequestException => (StatusCodes.Status502BadGateway, "An upstream service error occurred", (string?)null),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred", (string?)null)
        };

        return new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = GetProblemType(statusCode),
            Instance = httpContext.Request.Path,
            Detail = detail,
            Extensions =
            {
                ["traceId"] = httpContext.TraceIdentifier,
                ["timestamp"] = DateTime.UtcNow
            }
        };
    }

    private static string GetProblemType(int statusCode)
    {
        return statusCode switch
        {
            400 => "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            401 => "https://tools.ietf.org/html/rfc9110#section-15.5.2",
            403 => "https://tools.ietf.org/html/rfc9110#section-15.5.4",
            404 => "https://tools.ietf.org/html/rfc9110#section-15.5.5",
            409 => "https://tools.ietf.org/html/rfc9110#section-15.5.10",
            502 => "https://tools.ietf.org/html/rfc9110#section-15.6.3",
            _ => "https://tools.ietf.org/html/rfc9110#section-15.6.1"
        };
    }
}
