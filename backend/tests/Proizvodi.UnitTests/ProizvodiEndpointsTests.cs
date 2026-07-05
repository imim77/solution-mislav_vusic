using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Proizvodi.Api;
using Proizvodi.Api.Data;
using Proizvodi.Api.Features.Proizvodi;
using Proizvodi.Api.Models;

namespace Proizvodi.UnitTests;

public class ProizvodiEndpointHandlerTests
{
    [Fact]
    public async Task SearchProizvodi_WithoutQuery_ReturnsBadRequest()
    {
        var result = await ProizvodiEndpoints.GetProizvodByTextInput(
            new TestProductSource(),
            q: " ");

        var badRequest = Assert.IsType<BadRequest<string>>(result);
        Assert.Equal("Search term is required.", badRequest.Value);
    }

    [Fact]
    public async Task GetProizvod_WhenUpstreamReturnsNotFound_ThrowsNotFoundException()
    {
        var productSource = new TestProductSource
        {
            ProductException = new AppException.NotFoundException("Product", 123)
        };

        var exception = await Assert.ThrowsAsync<AppException.NotFoundException>(
            () => ProizvodiEndpoints.GetProizvodAsync(productSource, 123));

        Assert.Equal("Product with identifier '123' was not found.", exception.Message);
    }

    [Fact]
    public async Task PostUserCredentials_WhenLoginSucceeds_StoresUserAndReturnsPublicLoginResult()
    {
        await using var db = CreateDbContext();
        var factory = new TestHttpClientFactory("""
            {
              "id": 7,
              "username": "mislav",
              "firstName": "Mislav",
              "lastName": "Vusic",
              "email": "mislav@example.test",
              "image": "",
              "accessToken": "access-token",
              "refreshToken": "refresh-token"
            }
            """);

        var result = await ProizvodiEndpoints.PostUserCredentials(
            factory,
            new LoginRequest("mislav", "password", null),
            db);

        var ok = Assert.IsType<Ok<LoginResult>>(result);
        Assert.NotNull(ok.Value);
        Assert.Equal(7, ok.Value.Id);
        Assert.Equal("access-token", ok.Value.AccessToken);

        var user = await db.Users.SingleAsync();
        Assert.Equal("mislav@example.test", user.Email);
        Assert.Equal("refresh-token", user.RefreshToken);
    }

    [Fact]
    public async Task PostProductFavorite_WhenUserDoesNotExist_ReturnsNotFound()
    {
        await using var db = CreateDbContext();

        var result = await ProizvodiEndpoints.PostProductFavorite(CreateFavoriteRequest(), db);

        var notFound = Assert.IsType<NotFound<string>>(result);
        Assert.Equal("User with Id 7 not found.", notFound.Value);
    }

    [Fact]
    public async Task PostProductFavorite_WhenProductAlreadyFavorite_ReturnsConflict()
    {
        await using var db = CreateDbContext();
        db.Users.Add(CreateUser());
        db.Products.Add(CreateProduct());
        db.UserFavorites.Add(new UserFavorite { UserId = 7, ProductId = 15 });
        await db.SaveChangesAsync();

        var result = await ProizvodiEndpoints.PostProductFavorite(CreateFavoriteRequest(), db);

        var conflict = Assert.IsType<Conflict<string>>(result);
        Assert.Equal("Product is already in favorites.", conflict.Value);
    }

    [Fact]
    public async Task PostProductFavorite_WhenProductIsNew_CreatesFavoriteAndReturnsProduct()
    {
        await using var db = CreateDbContext();
        db.Users.Add(CreateUser());
        await db.SaveChangesAsync();

        var result = await ProizvodiEndpoints.PostProductFavorite(CreateFavoriteRequest(), db);

        var created = Assert.IsType<Created<ProizvodiDto>>(result);
        Assert.Equal("/proizvodi/15", created.Location);
        Assert.NotNull(created.Value);
        Assert.Equal("Keyboard", created.Value.Title);
        Assert.True(await db.UserFavorites.AnyAsync(f => f.UserId == 7 && f.ProductId == 15));
    }

    private static ProizvodiContext CreateDbContext()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ProizvodiContext>()
            .UseSqlite(connection)
            .Options;

        var db = new ProizvodiContext(options);
        db.Database.EnsureCreated();
        return db;
    }

    private static User CreateUser() => new()
    {
        Id = 7,
        Username = "mislav",
        FirstName = "Mislav",
        LastName = "Vusic",
        Email = "mislav@example.test",
        AccessToken = "access-token",
        RefreshToken = "refresh-token"
    };

    private static Product CreateProduct() => new()
    {
        Id = 15,
        Title = "Keyboard",
        Price = 49.99m,
        Description = "Mechanical keyboard",
        Thumbnail = "https://example.test/keyboard.png"
    };

    private static AddFavoriteRequest CreateFavoriteRequest() => new(
        UserId: 7,
        ProductId: 15,
        Title: "Keyboard",
        Price: 49.99m,
        Description: "Mechanical keyboard",
        Thumbnail: "https://example.test/keyboard.png");
}

internal sealed class TestHttpClientFactory : IHttpClientFactory
{
    private readonly string _json;
    private readonly HttpStatusCode _statusCode;

    public TestHttpClientFactory(string json, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        _json = json;
        _statusCode = statusCode;
    }

    public HttpClient CreateClient(string name)
    {
        return new HttpClient(new TestHttpMessageHandler(_json, _statusCode))
        {
            BaseAddress = new Uri("https://dummyjson.com")
        };
    }

    private sealed class TestHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _json;
        private readonly HttpStatusCode _statusCode;

        public TestHttpMessageHandler(string json, HttpStatusCode statusCode)
        {
            _json = json;
            _statusCode = statusCode;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_json, Encoding.UTF8, "application/json")
            });
        }
    }
}
