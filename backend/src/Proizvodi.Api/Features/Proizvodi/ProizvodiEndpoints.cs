using Proizvodi.Api.Features.Categories;

namespace Proizvodi.Api.Features.Proizvodi;

public static class ProizvodiEndpoints
{
    public static void MapProizvodiEndpoints(this WebApplication app)
    {
        app.MapGet("/proizvodi", GetProizvodi.GetProizvodiAsync);
        app.MapGet("/proizvodi/search", GetProizvodi.GetProizvodByTextInput);
        app.MapGet("/proizvodi/{id:int}", GetProizvodi.GetProizvodAsync);
        app.MapGet("/proizvodi/categories", GetCategories.GetCategoriesAsync);
        app.MapGet("/proizvodi/categories/{slug}", GetCategories.GetCategoyItems);
    }
}