using System.Net.Http.Json;
using WarehouseERP.Blazor.Infrastructure.Http;
using WarehouseERP.Shared.Contracts.StorageLocations;

namespace WarehouseERP.Blazor.Features.StorageLocations.Services;

public sealed class StorageLocationApiClient : IStorageLocationApiClient
{
    private const string BaseRoute = "api/storage-locations";

    private readonly HttpClient _httpClient;

    public StorageLocationApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<StorageLocationDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(BaseRoute, cancellationToken);
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var storageLocations = await response.Content.ReadFromJsonAsync<IReadOnlyList<StorageLocationDto>>(cancellationToken: cancellationToken);

        return storageLocations ?? [];
    }

    public async Task<StorageLocationDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{BaseRoute}/{id}", cancellationToken);

        return await ReadStorageLocationAsync(response, cancellationToken);
    }

    public async Task<IReadOnlyList<StorageLocationDto>> GetByWarehouseIdAsync(Guid warehouseId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"api/warehouses/{warehouseId}/storage-locations", cancellationToken);
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var storageLocations = await response.Content.ReadFromJsonAsync<IReadOnlyList<StorageLocationDto>>(cancellationToken: cancellationToken);

        return storageLocations ?? [];
    }

    public async Task<StorageLocationDto> CreateAsync(CreateStorageLocationRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseRoute, request, cancellationToken);

        return await ReadStorageLocationAsync(response, cancellationToken);
    }

    public async Task<StorageLocationDto> UpdateAsync(Guid id, UpdateStorageLocationRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseRoute}/{id}", request, cancellationToken);

        return await ReadStorageLocationAsync(response, cancellationToken);
    }

    public async Task<StorageLocationDto> ActivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PatchAsync($"{BaseRoute}/{id}/activate", null, cancellationToken);

        return await ReadStorageLocationAsync(response, cancellationToken);
    }

    public async Task<StorageLocationDto> DeactivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PatchAsync($"{BaseRoute}/{id}/deactivate", null, cancellationToken);

        return await ReadStorageLocationAsync(response, cancellationToken);
    }

    private static async Task<StorageLocationDto> ReadStorageLocationAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var storageLocation = await response.Content.ReadFromJsonAsync<StorageLocationDto>(cancellationToken: cancellationToken);

        return storageLocation ?? throw new ApiException((int)response.StatusCode, "The API returned an empty response.");
    }
}
