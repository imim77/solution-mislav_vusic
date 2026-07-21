using Proizvodi.Api.Features.Proizvodi;

namespace Proizvodi.Api.Features.Categories;

public static class GetCategories
{
    public static async Task<IResult> GetCategoriesAsync(
        IProductSource productSource,
        CancellationToken cancellationToken = default)
    {
        var categories = await productSource.GetCategoriesAsync(cancellationToken);
        return Results.Ok(categories);
    }

    public static async Task<IResult> GetCategoryItems(
        IProductSource productSource,
        string slug,
        decimal? minPrice,
        decimal? maxPrice,
        int? page = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default)
    {
        if (minPrice.HasValue && maxPrice.HasValue && minPrice.Value > maxPrice.Value)
        {
            return Results.BadRequest("minPrice cannot be greater than maxPrice.");
        }

        var products = await productSource.GetCategoryProductsAsync(slug, cancellationToken);

        var filteredProducts = products
            .Where(p => minPrice is null || p.Price >= minPrice.Value)
            .Where(p => maxPrice is null || p.Price <= maxPrice.Value)
            .ToList();

        if (!PaginationParams.TryCreate(page, pageSize, out var paging, out var error))
        {
            return Results.BadRequest(error);
        }

        if (paging is null)
        {
            return Results.Ok(filteredProducts);
        }

        return Results.Ok(PagedResult.Create(filteredProducts, paging.Page, paging.PageSize));
    }
}
