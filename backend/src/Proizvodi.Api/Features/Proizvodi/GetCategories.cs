using Proizvodi.Api.Features.Proizvodi;

namespace Proizvodi.Api.Features.Categories;

public static class GetCategories
{
    public static async Task<IResult> GetCategoriesAsync(IProductSource productSource)
    {
        var categories = await productSource.GetCategoriesAsync();
        return Results.Ok(categories);
    }

    public static async Task<IResult> GetCategoryItems(
        IProductSource productSource,
        string slug,
        decimal? minPrice,
        decimal? maxPrice)
    {
        var products = (await productSource.GetCategoryProductsAsync(slug)).AsEnumerable();

        if (minPrice.HasValue)
        {
            products = products.Where(p => p.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            products = products.Where(p => p.Price <= maxPrice.Value);
        }

        return Results.Ok(products.ToList());
    }
}
