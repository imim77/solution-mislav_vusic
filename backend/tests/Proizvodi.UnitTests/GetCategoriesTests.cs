using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Proizvodi.Api;
using Proizvodi.Api.Features.Categories;
using Proizvodi.Api.Features.Proizvodi;

namespace Proizvodi.UnitTests;

public class GetCategoriesTests
{
    [Fact]
    public async Task GetCategoryItems_WithPriceBounds_ReturnsOnlyProductsInsideBounds()
    {
        var factory = new TestHttpClientFactory("""
            {
              "products": [
                {
                  "id": 1,
                  "title": "Cheap phone",
                  "price": 99,
                  "description": "Entry phone",
                  "thumbnail": "https://example.test/cheap.png"
                },
                {
                  "id": 2,
                  "title": "Good phone",
                  "price": 599,
                  "description": "Daily phone",
                  "thumbnail": "https://example.test/good.png"
                },
                {
                  "id": 3,
                  "title": "Premium phone",
                  "price": 1299,
                  "description": "Flagship phone",
                  "thumbnail": "https://example.test/premium.png"
                }
              ],
              "total": 3,
              "skip": 0,
              "limit": 30
            }
            """);

        var result = await GetCategories.GetCategoryItems(
            factory,
            slug: "smartphones",
            minPrice: 500,
            maxPrice: 900);

        var ok = Assert.IsType<Ok<List<ProizvodiDto>>>(result);
        Assert.NotNull(ok.Value);
        var product = Assert.Single(ok.Value);
        Assert.Equal("Good phone", product.Title);
    }

    [Fact]
    public async Task GetCategoryItems_WhenUpstreamReturnsNotFound_ThrowsNotFoundException()
    {
        var factory = new TestHttpClientFactory("{}", HttpStatusCode.NotFound);

        var exception = await Assert.ThrowsAsync<AppException.NotFoundException>(
            () => GetCategories.GetCategoryItems(factory, "unknown", null, null));

        Assert.Equal("Category with identifier 'unknown' was not found.", exception.Message);
    }
}
