namespace Proizvodi.Api.Features.Proizvodi;

public static class ProizvodiEndpoints
{
    public static void MapProizvodiEndpoints(this WebApplication app)
    {
        app.MapGet("/proizvodi", GetProizvodi.GetProizvodiAsync);
    }
}