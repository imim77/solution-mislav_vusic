using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Proizvodi.Api;

namespace Proizvodi.UnitTests;

public class GlobalExceptionHandler_Tests
{
    private readonly FakeLogger _logger = new();

    [Fact]
    public async Task TryHandleAsync_WithAppException_SetsMappedStatusCodeAndReturnsTrue()
    {
        var httpContext = CreateHttpContext();
        var handler = CreateHandler(httpContext);
        var exception = new AppException.NotFoundException("Product", 123);

        var result = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

        var response = GetResponseJson(httpContext);
        Assert.True(result);
        Assert.Equal(StatusCodes.Status404NotFound, httpContext.Response.StatusCode);
        Assert.Equal(StatusCodes.Status404NotFound, response.GetProperty("status").GetInt32());
        Assert.Equal("Product with identifier '123' was not found.", response.GetProperty("title").GetString());
        Assert.Equal("Product with identifier '123' was not found.", response.GetProperty("detail").GetString());
        Assert.Equal(httpContext.TraceIdentifier, response.GetProperty("traceId").GetString());
        Assert.NotNull(response.GetProperty("timestamp").GetString());
        AssertLoggedOnce(LogLevel.Error, exception);
    }

    [Fact]
    public async Task TryHandleAsync_WithArgumentNullException_SetsBadRequestAndReturnsTrue()
    {
        var httpContext = CreateHttpContext();
        var handler = CreateHandler(httpContext);
        var exception = new ArgumentNullException("param");

        var result = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

        var response = GetResponseJson(httpContext);
        Assert.True(result);
        Assert.Equal(StatusCodes.Status400BadRequest, httpContext.Response.StatusCode);
        Assert.Equal(StatusCodes.Status400BadRequest, response.GetProperty("status").GetInt32());
        Assert.Equal("Invalid argument provided", response.GetProperty("title").GetString());
        Assert.False(response.TryGetProperty("detail", out _));
        AssertLoggedOnce(LogLevel.Error, exception);
    }

    [Fact]
    public async Task TryHandleAsync_WithArgumentException_SetsBadRequestAndReturnsTrue()
    {
        var httpContext = CreateHttpContext();
        var handler = CreateHandler(httpContext);
        var exception = new ArgumentException("Invalid argument");

        var result = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

        var response = GetResponseJson(httpContext);
        Assert.True(result);
        Assert.Equal(StatusCodes.Status400BadRequest, httpContext.Response.StatusCode);
        Assert.Equal(StatusCodes.Status400BadRequest, response.GetProperty("status").GetInt32());
        Assert.Equal("Invalid argument provided", response.GetProperty("title").GetString());
        Assert.False(response.TryGetProperty("detail", out _));
        AssertLoggedOnce(LogLevel.Error, exception);
    }

    

    private GlobalExceptionHandler CreateHandler(DefaultHttpContext httpContext)
    {
        var problemDetailsService = httpContext.RequestServices.GetRequiredService<IProblemDetailsService>();
        return new GlobalExceptionHandler(_logger, problemDetailsService);
    }

    private void AssertLoggedOnce(LogLevel level, Exception exception)
    {
        Assert.Single(_logger.Logs);
        Assert.Equal(level, _logger.Logs[0].Level);
        Assert.Same(exception, _logger.Logs[0].Exception);
    }

    private static DefaultHttpContext CreateHttpContext()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddProblemDetails();

        return new DefaultHttpContext
        {
            RequestServices = services.BuildServiceProvider(),
            Response = { Body = new MemoryStream() },
            Request = { Path = "/test" }
        };
    }

    private static JsonElement GetResponseJson(DefaultHttpContext httpContext)
    {
        httpContext.Response.Body.Position = 0;
        using var reader = new StreamReader(httpContext.Response.Body);
        var json = reader.ReadToEnd();
        return JsonDocument.Parse(json).RootElement;
    }

    private class FakeLogger : ILogger<GlobalExceptionHandler>
    {
        public List<(LogLevel Level, Exception? Exception)> Logs { get; } = new();

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            Logs.Add((logLevel, exception));
        }
    }
}
