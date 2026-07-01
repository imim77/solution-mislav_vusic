using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Proizvodi.Api;

namespace Proizvodi.UnitTests;

public class GlobalExceptionHandler_Tests
{
    [Fact]
    public async Task TryHandleAsync_GenericException_ReturnsTrueAndSetsInternalServerError()
    {        
        var logger = new FakeLogger();
        var handler = new GlobalExceptionHandler(logger);
        var httpContext = CreateHttpContext();
        var exception = new Exception("Something went wrong");

        var result = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

        Assert.True(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, httpContext.Response.StatusCode);
        Assert.Single(logger.Logs);
        Assert.Equal(LogLevel.Error, logger.Logs[0].Level);
        Assert.Same(exception, logger.Logs[0].Exception);
    }

    private static DefaultHttpContext CreateHttpContext()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddProblemDetails();

        return new DefaultHttpContext
        {
            RequestServices = services.BuildServiceProvider(),
            Response = { Body = new MemoryStream() }
        };
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
