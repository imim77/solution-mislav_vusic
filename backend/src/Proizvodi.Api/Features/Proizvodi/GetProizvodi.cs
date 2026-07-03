using System.Net;
using System.Net.Http.Json;
using Proizvodi.Api;

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
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new AppException.NotFoundException("Product", id);
        }
        response.EnsureSuccessStatusCode();
        var proizvod = await response.Content.ReadFromJsonAsync<ProizvodiDto>();
        return Results.Ok(proizvod);
    }
    public static async Task<IResult> GetProizvodByTextInput(IHttpClientFactory factory, string? q)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return Results.BadRequest("Search term is required.");
        }

        var http = factory.CreateClient("nesto");
        var response = await http.GetAsync($"/products/search?q={Uri.EscapeDataString(q)}");
        response.EnsureSuccessStatusCode();
        var proizvodi = await response.Content.ReadFromJsonAsync<ProizvodiResponse>();
        return Results.Ok(proizvodi?.Products);
    }

}
