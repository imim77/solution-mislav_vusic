using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Proizvodi.Api;
using Proizvodi.Api.Data;
using Proizvodi.Api.Models;

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
        var proizvod = await response.Content.ReadFromJsonAsync<ProductDetailsDto>();
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
    public static async Task<IResult> PostUserCredentials(IHttpClientFactory factory, LoginRequest request, ProizvodiContext dbContext)
    {
        var http = factory.CreateClient("nesto");
        var response = await http.PostAsJsonAsync("/auth/login", request);
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

        var result = new LoginResult(
            login.Id,
            login.Username,
            login.FirstName,
            login.LastName,
            login.AccessToken
        );

        return Results.Ok(result);
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
            product = new Product
            {
                Id = request.ProductId,
                Title = request.Title,
                Price = request.Price,
                Description = request.Description,
                Thumbnail = request.Thumbnail
            };
            dbContext.Products.Add(product);
        }

        var alreadyFavorite = await dbContext.UserFavorites
            .AnyAsync(uf => uf.UserId == request.UserId && uf.ProductId == request.ProductId);

        if (alreadyFavorite)
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

        return Results.Created($"/proizvodi/{request.ProductId}", favorite);
    }

}
