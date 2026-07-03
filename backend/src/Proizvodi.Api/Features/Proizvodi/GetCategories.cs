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
}