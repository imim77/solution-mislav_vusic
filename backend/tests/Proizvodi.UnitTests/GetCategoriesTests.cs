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
        var productSource = new TestProductSource
        {
            CategoryProducts =
            [
                new ProizvodiDto(
                    1,
                    "Cheap phone",
                    99,
                    "Entry phone",
                    "https://example.test/cheap.png"),
                new ProizvodiDto(
                    2,
                    "Good phone",
                    599,
                    "Daily phone",
                    "https://example.test/good.png"),
                new ProizvodiDto(
                    3,
                    "Premium phone",
                    1299,
                    "Flagship phone",
                    "https://example.test/premium.png")
            ]
        };

        var result = await GetCategories.GetCategoryItems(
            productSource,
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
        var productSource = new TestProductSource
        {
            CategoryException = new AppException.NotFoundException("Category", "unknown")
        };

        var exception = await Assert.ThrowsAsync<AppException.NotFoundException>(
            () => GetCategories.GetCategoryItems(productSource, "unknown", null, null));

        Assert.Equal("Category with identifier 'unknown' was not found.", exception.Message);
    }
}
