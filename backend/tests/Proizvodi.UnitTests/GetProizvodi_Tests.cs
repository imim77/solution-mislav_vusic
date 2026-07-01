using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Moq;
using Proizvodi.Api.Features.Proizvodi;

namespace Proizvodi.UnitTests;

public class GetProizvodi_Tests
{
    [Fact]
    public async Task GetProizvodiAsync_ValidResponse_ReturnsOkWithProducts()
    {
        
        var responseContent = new ProizvodiResponse(
            Products:
            [
                new ProizvodiDto(
                    Title: "Test Product",
                    Price: 9.99m,
                    Description: "Test description",
                    Thumbnail: "https://example.com/image.jpg")
            ],
            Total: 1,
            Skip: 0,
            Limit: 30
        );

        var factory = CreateHttpClientFactory(responseContent);

        
        var result = await GetProizvodi.GetProizvodiAsync(factory);

        
        var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status200OK, statusCodeResult.StatusCode);

        var valueResult = Assert.IsAssignableFrom<IValueHttpResult>(result);
        var products = Assert.IsType<List<ProizvodiDto>>(valueResult.Value);
        Assert.Single(products);
        Assert.Equal("Test Product", products[0].Title);
        Assert.Equal(9.99m, products[0].Price);
        Assert.Equal("Test description", products[0].Description);
        Assert.Equal("https://example.com/image.jpg", products[0].Thumbnail);
    }

    [Fact]
    public async Task GetProizvodAsync_ValidResponse_ReturnsOkWithProduct()
    {
        var responseContent = new ProizvodiDto(
            Title: "Test Product",
            Price: 9.99m,
            Description: "Test description",
            Thumbnail: "https://example.com/image.jpg"
        );

        var factory = CreateHttpClientFactory(responseContent);

        var result = await GetProizvodi.GetProizvodAsync(factory, 1);

        var statusCodeResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status200OK, statusCodeResult.StatusCode);

        var valueResult = Assert.IsAssignableFrom<IValueHttpResult>(result);
        var product = Assert.IsType<ProizvodiDto>(valueResult.Value);
        Assert.Equal("Test Product", product.Title);
        Assert.Equal(9.99m, product.Price);
        Assert.Equal("Test description", product.Description);
        Assert.Equal("https://example.com/image.jpg", product.Thumbnail);
    }

    private static IHttpClientFactory CreateHttpClientFactory(ProizvodiResponse response)
    {
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return CreateHttpClientFactory(json);
    }

    private static IHttpClientFactory CreateHttpClientFactory(ProizvodiDto response)
    {
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return CreateHttpClientFactory(json);
    }

    private static IHttpClientFactory CreateHttpClientFactory(string json)
    {
        var handler = new FakeHttpMessageHandler(json);
        var client = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://dummyjson.com")
        };

        var factory = new Mock<IHttpClientFactory>();
        factory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);

        return factory.Object;
    }

    private class FakeHttpMessageHandler(string responseContent) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseContent)
            });
        }
    }
}
