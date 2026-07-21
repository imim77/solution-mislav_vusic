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
        var group = app.MapGroup("/proizvodi");

        group.MapGet("/", GetProizvodiAsync);
        group.MapGet("/search", GetProizvodByTextInput);
        group.MapGet("/{id:int}", GetProizvodAsync);
        group.MapGet("/categories", GetCategories.GetCategoriesAsync);
        group.MapGet("/categories/{slug}", GetCategories.GetCategoryItems);
        group.MapPost("/login", PostUserCredentials);
        group.MapPost("/favorites", PostProductFavorite);
        group.MapGet("/favorites/{userId:int}", GetUserFavorites);
    }

    public static async Task<IResult> GetProizvodiAsync(
        IProductSource productSource,
        CancellationToken cancellationToken = default)
    {
        var products = await productSource.GetProductsAsync(cancellationToken);
        return Results.Ok(products);
    }

    public static async Task<IResult> GetProizvodAsync(
        IProductSource productSource,
        int id,
        CancellationToken cancellationToken = default)
    {
        var product = await productSource.GetProductAsync(id, cancellationToken);
        return Results.Ok(product);
    }

    public static async Task<IResult> GetProizvodByTextInput(
        IProductSource productSource,
        string? q,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return Results.BadRequest("Search term is required.");
        }

        var products = await productSource.SearchProductsAsync(q, cancellationToken);
        return Results.Ok(products);
    }

    public static async Task<IResult> PostUserCredentials(
        IHttpClientFactory httpClientFactory,
        LoginRequest request,
        ProizvodiContext dbContext,
        CancellationToken cancellationToken = default)
    {
        var httpClient = httpClientFactory.CreateDummyJsonClient();
        var response = await httpClient.PostAsJsonAsync("/auth/login", request, cancellationToken);
        response.EnsureSuccessStatusCode();
        var login = await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken);
        if (login is null)
        {
            return Results.BadRequest("Invalid login response.");
        }

        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == login.Id, cancellationToken);

        if (user is null)
        {
            dbContext.Users.Add(login.ToUser());
        }
        else
        {
            user.UpdateTokens(login);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(login.ToLoginResult());
    }

    public static async Task<IResult> PostProductFavorite(
        AddFavoriteRequest request,
        ProizvodiContext dbContext,
        CancellationToken cancellationToken = default)
    {
        if (!await UserExistsAsync(dbContext, request.UserId, cancellationToken))
        {
            return Results.NotFound(UserNotFoundMessage(request.UserId));
        }

        var product = await dbContext.Products.FindAsync([request.ProductId], cancellationToken);
        if (product is null)
        {
            product = request.ToProduct();
            dbContext.Products.Add(product);
        }

        var isAlreadyFavorite = await dbContext.UserFavorites
            .AnyAsync(uf => uf.UserId == request.UserId && uf.ProductId == request.ProductId, cancellationToken);

        if (isAlreadyFavorite)
        {
            return Results.Conflict("Product is already in favorites.");
        }

        dbContext.UserFavorites.Add(new UserFavorite
        {
            UserId = request.UserId,
            ProductId = request.ProductId
        });
        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Created($"/proizvodi/{request.ProductId}", product.ToDto());
    }

    public static async Task<IResult> GetUserFavorites(
        int userId,
        ProizvodiContext dbContext,
        CancellationToken cancellationToken = default)
    {
        if (!await UserExistsAsync(dbContext, userId, cancellationToken))
        {
            return Results.NotFound(UserNotFoundMessage(userId));
        }

        var favorites = await dbContext.UserFavorites
            .Where(uf => uf.UserId == userId)
            .Select(uf => uf.Product)
            .Select(ProductMappingExtensions.ToDtoProjection)
            .ToListAsync(cancellationToken);

        return Results.Ok(favorites);
    }

    private static async Task<bool> UserExistsAsync(
        ProizvodiContext dbContext,
        int userId,
        CancellationToken cancellationToken)
    {
        return await dbContext.Users.AnyAsync(u => u.Id == userId, cancellationToken);
    }

    private static string UserNotFoundMessage(int userId) => $"User with Id {userId} not found.";
}
