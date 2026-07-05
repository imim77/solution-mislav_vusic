using System.Net;
using System.Net.Http.Json;
using Proizvodi.Api.Features.Categories;
using Proizvodi.Api.Features.Proizvodi;

namespace Proizvodi.IntegrationTests;

public sealed class ProizvodiEndpointsTests : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client;

    public ProizvodiEndpointsTests(IntegrationTestFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetProizvodi_ReturnsProductsFromUpstream()
    {
        var products = await _client.GetFromJsonAsync<List<ProizvodiDto>>("/proizvodi");

        Assert.NotNull(products);
        Assert.Collection(
            products,
            product => Assert.Equal("iPhone 15", product.Title),
            product => Assert.Equal("Galaxy S24", product.Title));
    }

    [Fact]
    public async Task GetProizvod_WhenUpstreamProductDoesNotExist_ReturnsNotFoundProblem()
    {
        var response = await _client.GetAsync("/proizvodi/999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>();
        Assert.Equal("Product with identifier '999' was not found.", problem?.Title);
    }

    [Fact]
    public async Task SearchProizvodi_WithoutQuery_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/proizvodi/search");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetCategories_ReturnsCategoriesFromUpstream()
    {
        var categories = await _client.GetFromJsonAsync<List<CategoriesDto>>("/proizvodi/categories");

        Assert.NotNull(categories);
        Assert.Contains(categories, category => category.Slug == "smartphones");
        Assert.Contains(categories, category => category.Slug == "laptops");
    }

    [Fact]
    public async Task GetCategoryItems_WithPriceFilters_ReturnsFilteredProducts()
    {
        var products = await _client.GetFromJsonAsync<List<ProizvodiDto>>(
            "/proizvodi/categories/smartphones?minPrice=500&maxPrice=900");

        var product = Assert.Single(products!);
        Assert.Equal("iPhone 15", product.Title);
    }

    [Fact]
    public async Task LoginAndFavorites_PersistsUserAndFavoriteProduct()
    {
        var loginResponse = await _client.PostAsJsonAsync(
            "/proizvodi/login",
            new LoginRequest("kminchelle", "password", null));
        loginResponse.EnsureSuccessStatusCode();

        var login = await loginResponse.Content.ReadFromJsonAsync<LoginResult>();

        var favoriteResponse = await _client.PostAsJsonAsync(
            "/proizvodi/favorites",
            new AddFavoriteRequest(
                login!.Id,
                1,
                "iPhone 15",
                799,
                "Apple phone",
                "https://cdn.example.test/iphone.png"));

        Assert.Equal(HttpStatusCode.Created, favoriteResponse.StatusCode);

        var favorites = await _client.GetFromJsonAsync<List<ProizvodiDto>>($"/proizvodi/favorites/{login.Id}");
        var favorite = Assert.Single(favorites!);
        Assert.Equal("iPhone 15", favorite.Title);
    }

    private sealed record ProblemDetailsResponse(string Title);
}
