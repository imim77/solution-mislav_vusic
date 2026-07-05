using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Proizvodi.Api.Data;
using Proizvodi.Api.Features.Categories;
using Proizvodi.Api.Infrastructure;
using Proizvodi.Api.Models;

namespace Proizvodi.Api.Features.Proizvodi;

public static class ProizvodiEndpoints
{
    public static void MapProizvodiEndpoints(this WebApplication app)
    {
        app.MapGet("/proizvodi", GetProizvodiAsync);
        app.MapGet("/proizvodi/search", GetProizvodByTextInput);
        app.MapGet("/proizvodi/{id:int}", GetProizvodAsync);
        app.MapGet("/proizvodi/categories", GetCategories.GetCategoriesAsync);
        app.MapGet("/proizvodi/categories/{slug}", GetCategories.GetCategoryItems);
        app.MapPost("/proizvodi/login", PostUserCredentials);
        app.MapPost("/proizvodi/favorites", PostProductFavorite);
        app.MapGet("/proizvodi/favorites/{userId:int}", GetUserFavorites);
    }

    public static async Task<IResult> GetProizvodiAsync(IProductSource productSource)
    {
        var products = await productSource.GetProductsAsync();
        return Results.Ok(products);
    }

    public static async Task<IResult> GetProizvodAsync(IProductSource productSource, int id)
    {
        var product = await productSource.GetProductAsync(id);
        return Results.Ok(product);
    }

    public static async Task<IResult> GetProizvodByTextInput(IProductSource productSource, string? q)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return Results.BadRequest("Search term is required.");
        }

        var products = await productSource.SearchProductsAsync(q);
        return Results.Ok(products);
    }

    public static async Task<IResult> PostUserCredentials(
        IHttpClientFactory httpClientFactory,
        LoginRequest request,
        ProizvodiContext dbContext)
    {
        var httpClient = httpClientFactory.CreateDummyJsonClient();
        var response = await httpClient.PostAsJsonAsync("/auth/login", request);
        response.EnsureSuccessStatusCode();
        var login = await response.Content.ReadFromJsonAsync<LoginResponse>();
        if (login is null)
        {
            return Results.BadRequest("Invalid login response.");
        }

        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == login.Id);

        if (user is null)
        {
            user = new User
            {
                Id = login.Id,
                Username = login.Username,
                FirstName = login.FirstName,
                LastName = login.LastName,
                Email = login.Email,
                AccessToken = login.AccessToken,
                RefreshToken = login.RefreshToken
            };
            dbContext.Users.Add(user);
        }
        else
        {
            user.AccessToken = login.AccessToken;
            user.RefreshToken = login.RefreshToken;
        }

        await dbContext.SaveChangesAsync();

        return Results.Ok(login.ToLoginResult());
    }

    public static async Task<IResult> PostProductFavorite(AddFavoriteRequest request, ProizvodiContext dbContext)
    {
        var userExists = await dbContext.Users.AnyAsync(u => u.Id == request.UserId);
        if (!userExists)
        {
            return Results.NotFound($"User with Id {request.UserId} not found.");
        }

        var product = await dbContext.Products.FindAsync(request.ProductId);
        if (product is null)
        {
            product = request.ToProduct();
            dbContext.Products.Add(product);
        }

        var isAlreadyFavorite = await dbContext.UserFavorites
            .AnyAsync(uf => uf.UserId == request.UserId && uf.ProductId == request.ProductId);

        if (isAlreadyFavorite)
        {
            return Results.Conflict("Product is already in favorites.");
        }

        var favorite = new UserFavorite
        {
            UserId = request.UserId,
            ProductId = request.ProductId
        };

        dbContext.UserFavorites.Add(favorite);
        await dbContext.SaveChangesAsync();

        return Results.Created($"/proizvodi/{request.ProductId}", product.ToDto());
    }

    public static async Task<IResult> GetUserFavorites(int userId, ProizvodiContext dbContext)
    {
        var userExists = await dbContext.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
        {
            return Results.NotFound($"User with Id {userId} not found.");
        }

        var favorites = await dbContext.UserFavorites
            .Where(uf => uf.UserId == userId)
            .Select(uf => uf.Product)
            .Select(ProductMappingExtensions.ToDtoProjection)
            .ToListAsync();

        return Results.Ok(favorites);
    }
}
