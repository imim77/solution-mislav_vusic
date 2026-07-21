using Proizvodi.Api.Features.Categories;

namespace Proizvodi.Api.Features.Proizvodi;

public interface IProductSource
{
    Task<IReadOnlyList<ProizvodiDto>> GetProductsAsync(CancellationToken cancellationToken = default);

    async Task<PagedResult<ProizvodiDto>> GetProductsPageAsync(
        PaginationParams paging,
        CancellationToken cancellationToken = default)
    {
        var products = await GetProductsAsync(cancellationToken);
        return PagedResult.Create(products, paging.Page, paging.PageSize);
    }

    Task<ProductDetailsDto> GetProductAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProizvodiDto>> SearchProductsAsync(string searchTerm, CancellationToken cancellationToken = default);

    async Task<PagedResult<ProizvodiDto>> SearchProductsPageAsync(
        string searchTerm,
        PaginationParams paging,
        CancellationToken cancellationToken = default)
    {
        var products = await SearchProductsAsync(searchTerm, cancellationToken);
        return PagedResult.Create(products, paging.Page, paging.PageSize);
    }

    Task<IReadOnlyList<CategoriesDto>> GetCategoriesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProizvodiDto>> GetCategoryProductsAsync(
        string slug,
        CancellationToken cancellationToken = default);
}
