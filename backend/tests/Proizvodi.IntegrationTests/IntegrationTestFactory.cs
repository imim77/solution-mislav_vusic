using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Proizvodi.Api.Data;

namespace Proizvodi.IntegrationTests;

public sealed class IntegrationTestFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<ProizvodiContext>>();

            services.AddSingleton(_ =>
            {
                var connection = new SqliteConnection("Data Source=:memory:");
                connection.Open();
                return connection;
            });

            services.AddDbContext<ProizvodiContext>((sp, options) =>
                options.UseSqlite(sp.GetRequiredService<SqliteConnection>()));

            services.AddHttpClient("nesto")
                .ConfigurePrimaryHttpMessageHandler(_ => new DummyJsonHandler());
        });
    }
}

internal sealed class DummyJsonHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var pathAndQuery = request.RequestUri?.PathAndQuery ?? string.Empty;

        var response = pathAndQuery switch
        {
            "/products" => Ok("""
                {
                  "products": [
                    {
                      "id": 1,
                      "title": "iPhone 15",
                      "price": 799,
                      "description": "Apple phone",
                      "thumbnail": "https://cdn.example.test/iphone.png"
                    },
                    {
                      "id": 2,
                      "title": "Galaxy S24",
                      "price": 699,
                      "description": "Samsung phone",
                      "thumbnail": "https://cdn.example.test/galaxy.png"
                    }
                  ],
                  "total": 2,
                  "skip": 0,
                  "limit": 30
                }
                """),
            "/products/1" => Ok("""
                {
                  "id": 1,
                  "title": "iPhone 15",
                  "price": 799,
                  "description": "Apple phone",
                  "category": "smartphones",
                  "brand": "Apple",
                  "rating": 4.7,
                  "stock": 12,
                  "availabilityStatus": "In Stock",
                  "thumbnail": "https://cdn.example.test/iphone.png",
                  "images": ["https://cdn.example.test/iphone.png"],
                  "tags": ["phone"],
                  "reviews": []
                }
                """),
            "/products/999" => Json(HttpStatusCode.NotFound, """{"message":"Product not found"}"""),
            "/products/search?q=phone" => Ok("""
                {
                  "products": [
                    {
                      "id": 1,
                      "title": "iPhone 15",
                      "price": 799,
                      "description": "Apple phone",
                      "thumbnail": "https://cdn.example.test/iphone.png"
                    }
                  ],
                  "total": 1,
                  "skip": 0,
                  "limit": 30
                }
                """),
            "/products/categories" => Ok("""
                [
                  { "name": "Smartphones", "slug": "smartphones" },
                  { "name": "Laptops", "slug": "laptops" }
                ]
                """),
            "/products/category/smartphones" => Ok("""
                {
                  "products": [
                    {
                      "id": 1,
                      "title": "iPhone 15",
                      "price": 799,
                      "description": "Apple phone",
                      "thumbnail": "https://cdn.example.test/iphone.png"
                    },
                    {
                      "id": 2,
                      "title": "Budget Phone",
                      "price": 199,
                      "description": "Affordable phone",
                      "thumbnail": "https://cdn.example.test/budget.png"
                    }
                  ],
                  "total": 2,
                  "skip": 0,
                  "limit": 30
                }
                """),
            "/products/category/unknown" => Json(HttpStatusCode.NotFound, """{"message":"Category not found"}"""),
            "/auth/login" => Ok("""
                {
                  "id": 15,
                  "username": "kminchelle",
                  "firstName": "Jeanne",
                  "lastName": "Halvorson",
                  "email": "jeanne@example.test",
                  "image": "https://cdn.example.test/user.png",
                  "accessToken": "access-token",
                  "refreshToken": "refresh-token"
                }
                """),
            _ => Json(HttpStatusCode.NotFound, """{"message":"Not found"}""")
        };

        return Task.FromResult(response);
    }

    private static HttpResponseMessage Ok(string json) => Json(HttpStatusCode.OK, json);

    private static HttpResponseMessage Json(HttpStatusCode statusCode, string json)
    {
        return new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
        };
    }
}
