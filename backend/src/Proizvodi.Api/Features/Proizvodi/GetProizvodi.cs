using System.Net.Http.Json;

namespace Proizvodi.Api.Features.Proizvodi;

public static class GetProizvodi
{
    public static async Task<IResult> GetProizvodiAsync(IHttpClientFactory factory)
    {
        var http = factory.CreateClient("nesto");
        var response = await http.GetAsync("/products");
        response.EnsureSuccessStatusCode();
        var proizvodi = await response.Content.ReadFromJsonAsync<ProizvodiResponse>();
        return Results.Ok(proizvodi?.Products);
    }
 
    public static async Task<IResult> GetProizvodAsync(IHttpClientFactory factory, int id)
    {
        var http = factory.CreateClient("nesto");
        var response = await http.GetAsync($"/products/{id}");
        response.EnsureSuccessStatusCode();
        var proizvod = await response.Content.ReadFromJsonAsync<ProizvodiDto>();
        return Results.Ok(proizvod);
    }
}
