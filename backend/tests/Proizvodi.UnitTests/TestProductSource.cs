using Proizvodi.Api.Features.Categories;
using Proizvodi.Api.Features.Proizvodi;

namespace Proizvodi.UnitTests;

internal sealed class TestProductSource : IProductSource
{
    public Exception? ProductException { get; init; }
    public Exception? CategoryException { get; init; }
    public IReadOnlyList<ProizvodiDto> Products { get; init; } = [];
    public IReadOnlyList<ProizvodiDto> SearchResults { get; init; } = [];
    public IReadOnlyList<ProizvodiDto> CategoryProducts { get; init; } = [];
    public IReadOnlyList<CategoriesDto> Categories { get; init; } = [];

    public Task<IReadOnlyList<ProizvodiDto>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Products);
    }

    public Task<ProductDetailsDto> GetProductAsync(int id, CancellationToken cancellationToken = default)
    {
        if (ProductException is not null)
        {
            throw ProductException;
        }

        return Task.FromResult(new ProductDetailsDto(
            id,
            "Test product",
            10,
            "Test description",
            "test-category",
            "Test brand",
            4.5,
            10,
            "In Stock",
            "https://example.test/product.png",
            [],
            [],
            []));
    }

    public Task<IReadOnlyList<ProizvodiDto>> SearchProductsAsync(
        string searchTerm,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(SearchResults);
    }

    public Task<IReadOnlyList<CategoriesDto>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Categories);
    }

    public Task<IReadOnlyList<ProizvodiDto>> GetCategoryProductsAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        if (CategoryException is not null)
        {
            throw CategoryException;
        }

        return Task.FromResult(CategoryProducts);
    }
}
