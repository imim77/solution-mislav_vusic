using Proizvodi.Api.Features.Proizvodi;

namespace Proizvodi.Api.Features.Categories;

public class GetCategories
{
    public static async Task<IResult> GetCategoriesAsync(IHttpClientFactory factory)
    {
        var http = factory.CreateClient("nesto");
        var response = await http.GetAsync("/products/categories");
        response.EnsureSuccessStatusCode();
        var proizvodi = await response.Content.ReadFromJsonAsync<List<CategoriesDto>>();
        return Results.Ok(proizvodi);
    }
    public static async Task<IResult> GetCategoyItems(IHttpClientFactory factory, string slug, decimal? minPrice, decimal? maxPrice)
    {
        var http = factory.CreateClient("nesto");
        var response = await http.GetAsync($"/products/category/{slug}");
        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadFromJsonAsync<ProizvodiResponse>();

        var products = data?.Products ?? [];

        if (minPrice.HasValue)
        products = products.Where(p => p.Price >= minPrice.Value).ToList();

        if (maxPrice.HasValue)
        products = products.Where(p => p.Price <= maxPrice.Value).ToList();

        return Results.Ok(products);
 
    }
}