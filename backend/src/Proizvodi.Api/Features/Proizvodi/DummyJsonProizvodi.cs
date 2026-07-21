using System.Net;
using Proizvodi.Api.Features.Categories;
using Proizvodi.Api.Infrastructure;

namespace Proizvodi.Api.Features.Proizvodi;

public sealed class DummyJsonProductSource(IHttpClientFactory httpClientFactory) : IProductSource
{
    public async Task<IReadOnlyList<ProizvodiDto>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        var products = await GetProductsEnvelopeAsync(
            "/products",
            "DummyJSON products response was empty.",
            cancellationToken);

        return products.Products;
    }

    public async Task<PagedResult<ProizvodiDto>> GetProductsPageAsync(
        PaginationParams paging,
        CancellationToken cancellationToken = default)
    {
        return await GetPagedProductsAsync(
            $"/products?limit={paging.PageSize}&skip={paging.Skip}",
            paging,
            "DummyJSON products response was empty.",
            cancellationToken);
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
        var products = await GetProductsEnvelopeAsync(
            $"/products/search?q={Uri.EscapeDataString(searchTerm)}",
            "DummyJSON search response was empty.",
            cancellationToken);

        return products.Products;
    }

    public async Task<PagedResult<ProizvodiDto>> SearchProductsPageAsync(
        string searchTerm,
        PaginationParams paging,
        CancellationToken cancellationToken = default)
    {
        return await GetPagedProductsAsync(
            $"/products/search?q={Uri.EscapeDataString(searchTerm)}&limit={paging.PageSize}&skip={paging.Skip}",
            paging,
            "DummyJSON search response was empty.",
            cancellationToken);
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
        var response = await GetAsync($"/products/category/{Uri.EscapeDataString(slug)}", cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new AppException.NotFoundException("Category", slug);
        }

        var products = await ReadProductsEnvelopeAsync(
            response,
            $"DummyJSON category response for '{slug}' was empty.",
            cancellationToken);

        return products.Products;
    }

    private async Task<ProizvodiResponse> GetProductsEnvelopeAsync(
        string requestUri,
        string errorMessage,
        CancellationToken cancellationToken)
    {
        var response = await GetAsync(requestUri, cancellationToken);
        return await ReadProductsEnvelopeAsync(response, errorMessage, cancellationToken);
    }

    private async Task<PagedResult<ProizvodiDto>> GetPagedProductsAsync(
        string requestUri,
        PaginationParams paging,
        string errorMessage,
        CancellationToken cancellationToken)
    {
        var products = await GetProductsEnvelopeAsync(requestUri, errorMessage, cancellationToken);
        return new PagedResult<ProizvodiDto>(products.Products, products.Total, paging.Page, paging.PageSize);
    }

    private static async Task<ProizvodiResponse> ReadProductsEnvelopeAsync(
        HttpResponseMessage response,
        string errorMessage,
        CancellationToken cancellationToken)
    {
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadRequiredJsonAsync<ProizvodiResponse>(errorMessage, cancellationToken);
    }

    private Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken)
    {
        var httpClient = httpClientFactory.CreateDummyJsonClient();
        return httpClient.GetAsync(requestUri, cancellationToken);
    }
}
