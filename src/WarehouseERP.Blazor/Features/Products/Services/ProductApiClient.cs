using System.Net.Http.Json;
using WarehouseERP.Blazor.Infrastructure.Http;
using WarehouseERP.Shared.Contracts.Products;

namespace WarehouseERP.Blazor.Features.Products.Services;

public sealed class ProductApiClient : IProductApiClient
{
    private const string BaseRoute = "api/products";

    private readonly HttpClient _httpClient;

    public ProductApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(BaseRoute, cancellationToken);
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var products = await response.Content.ReadFromJsonAsync<IReadOnlyList<ProductDto>>(cancellationToken: cancellationToken);

        return products ?? [];
    }

    public async Task<ProductDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{BaseRoute}/{id}", cancellationToken);

        return await ReadProductAsync(response, cancellationToken);
    }

    public async Task<ProductDto> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseRoute, request, cancellationToken);

        return await ReadProductAsync(response, cancellationToken);
    }

    public async Task<ProductDto> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseRoute}/{id}", request, cancellationToken);

        return await ReadProductAsync(response, cancellationToken);
    }

    public async Task<ProductDto> ActivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PatchAsync($"{BaseRoute}/{id}/activate", null, cancellationToken);

        return await ReadProductAsync(response, cancellationToken);
    }

    public async Task<ProductDto> DeactivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PatchAsync($"{BaseRoute}/{id}/deactivate", null, cancellationToken);

        return await ReadProductAsync(response, cancellationToken);
    }

    private static async Task<ProductDto> ReadProductAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var product = await response.Content.ReadFromJsonAsync<ProductDto>(cancellationToken: cancellationToken);

        return product ?? throw new ApiException((int)response.StatusCode, "The API returned an empty response.");
    }
}
