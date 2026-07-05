using Proizvodi.Api.Features.Categories;

namespace Proizvodi.Api.Features.Proizvodi;

public interface IProductSource
{
    Task<IReadOnlyList<ProizvodiDto>> GetProductsAsync(CancellationToken cancellationToken = default);

    Task<ProductDetailsDto> GetProductAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProizvodiDto>> SearchProductsAsync(string searchTerm, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CategoriesDto>> GetCategoriesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProizvodiDto>> GetCategoryProductsAsync(
        string slug,
        CancellationToken cancellationToken = default);
}
