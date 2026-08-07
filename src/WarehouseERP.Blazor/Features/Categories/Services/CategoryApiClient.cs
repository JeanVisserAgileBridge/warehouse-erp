using System.Net.Http.Json;
using WarehouseERP.Blazor.Infrastructure.Http;
using WarehouseERP.Shared.Contracts.Categories;

namespace WarehouseERP.Blazor.Features.Categories.Services;

public sealed class CategoryApiClient : ICategoryApiClient
{
    private const string BaseRoute = "api/categories";

    private readonly HttpClient _httpClient;

    public CategoryApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(BaseRoute, cancellationToken);
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var categories = await response.Content.ReadFromJsonAsync<IReadOnlyList<CategoryDto>>(cancellationToken: cancellationToken);

        return categories ?? [];
    }

    public async Task<CategoryDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{BaseRoute}/{id}", cancellationToken);

        return await ReadCategoryAsync(response, cancellationToken);
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseRoute, request, cancellationToken);

        return await ReadCategoryAsync(response, cancellationToken);
    }

    public async Task<CategoryDto> UpdateAsync(Guid id, UpdateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseRoute}/{id}", request, cancellationToken);

        return await ReadCategoryAsync(response, cancellationToken);
    }

    public async Task<CategoryDto> ActivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PatchAsync($"{BaseRoute}/{id}/activate", null, cancellationToken);

        return await ReadCategoryAsync(response, cancellationToken);
    }

    public async Task<CategoryDto> DeactivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PatchAsync($"{BaseRoute}/{id}/deactivate", null, cancellationToken);

        return await ReadCategoryAsync(response, cancellationToken);
    }

    private static async Task<CategoryDto> ReadCategoryAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var category = await response.Content.ReadFromJsonAsync<CategoryDto>(cancellationToken: cancellationToken);

        return category ?? throw new ApiException((int)response.StatusCode, "The API returned an empty response.");
    }
}
