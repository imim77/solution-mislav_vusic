using System.Net;
using Proizvodi.Api.Features.Categories;
using Proizvodi.Api.Infrastructure;

namespace Proizvodi.Api.Features.Proizvodi;

public sealed class DummyJsonProductSource(IHttpClientFactory httpClientFactory) : IProductSource
{
    public async Task<IReadOnlyList<ProizvodiDto>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        var response = await GetAsync("/products", cancellationToken);
        response.EnsureSuccessStatusCode();

        var products = await response.Content.ReadRequiredJsonAsync<ProizvodiResponse>(
            "DummyJSON products response was empty.",
            cancellationToken);

        return products.Products;
    }

    public async Task<ProductDetailsDto> GetProductAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await GetAsync($"/products/{id}", cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new AppException.NotFoundException("Product", id);
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadRequiredJsonAsync<ProductDetailsDto>(
            $"DummyJSON product response for '{id}' was empty.",
            cancellationToken);
    }

    public async Task<IReadOnlyList<ProizvodiDto>> SearchProductsAsync(
        string searchTerm,
        CancellationToken cancellationToken = default)
    {
        var escapedSearchTerm = Uri.EscapeDataString(searchTerm);
        var response = await GetAsync($"/products/search?q={escapedSearchTerm}", cancellationToken);
        response.EnsureSuccessStatusCode();

        var products = await response.Content.ReadRequiredJsonAsync<ProizvodiResponse>(
            "DummyJSON search response was empty.",
            cancellationToken);

        return products.Products;
    }

    public async Task<IReadOnlyList<CategoriesDto>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var response = await GetAsync("/products/categories", cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadRequiredJsonAsync<List<CategoriesDto>>(
            "DummyJSON categories response was empty.",
            cancellationToken);
    }

    public async Task<IReadOnlyList<ProizvodiDto>> GetCategoryProductsAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        var response = await GetAsync($"/products/category/{slug}", cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new AppException.NotFoundException("Category", slug);
        }

        response.EnsureSuccessStatusCode();

        var products = await response.Content.ReadRequiredJsonAsync<ProizvodiResponse>(
            $"DummyJSON category response for '{slug}' was empty.",
            cancellationToken);

        return products.Products;
    }

    private Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken)
    {
        var httpClient = httpClientFactory.CreateDummyJsonClient();
        return httpClient.GetAsync(requestUri, cancellationToken);
    }
}
