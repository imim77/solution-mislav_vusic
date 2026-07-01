namespace Proizvodi.Api.Features.Proizvodi;

public static class GetProizvodi
{
    public static async Task<IResult> GetProizvodiAsync(IHttpClientFactory factory)
    {
        var http = factory.CreateClient("nesto");
        var response = await http.GetAsync("/products");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return Results.Content(json, "application/json");
    }
}