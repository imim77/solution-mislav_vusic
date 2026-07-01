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
}
