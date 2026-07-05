using System.Net.Http.Json;
using Proizvodi.Api.Features.Proizvodi;

namespace Proizvodi.Api.Infrastructure;

public static class DummyJsonClientExtensions
{
    public const string ClientName = "DummyJson";
    private const string DefaultBaseAddress = "https://dummyjson.com";

    public static IServiceCollection AddDummyJsonClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var baseAddress = configuration["DummyJson:BaseAddress"] ?? DefaultBaseAddress;

        services.AddHttpClient(ClientName, client =>
        {
            client.BaseAddress = new Uri(baseAddress);
        });
        services.AddScoped<IProductSource, DummyJsonProductSource>();

        return services;
    }

    public static HttpClient CreateDummyJsonClient(this IHttpClientFactory httpClientFactory)
    {
        return httpClientFactory.CreateClient(ClientName);
    }

    public static async Task<T> ReadRequiredJsonAsync<T>(
        this HttpContent content,
        string errorMessage,
        CancellationToken cancellationToken = default)
    {
        var value = await content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
        return value ?? throw new InvalidOperationException(errorMessage);
    }
}
