using System.Net.Http.Json;
using WarehouseERP.Blazor.Infrastructure.Http;
using WarehouseERP.Shared.Contracts.Warehouses;

namespace WarehouseERP.Blazor.Features.Warehouses.Services;

public sealed class WarehouseApiClient : IWarehouseApiClient
{
    private const string BaseRoute = "api/warehouses";

    private readonly HttpClient _httpClient;

    public WarehouseApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<WarehouseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(BaseRoute, cancellationToken);
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var warehouses = await response.Content.ReadFromJsonAsync<IReadOnlyList<WarehouseDto>>(cancellationToken: cancellationToken);

        return warehouses ?? [];
    }

    public async Task<WarehouseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{BaseRoute}/{id}", cancellationToken);

        return await ReadWarehouseAsync(response, cancellationToken);
    }

    public async Task<WarehouseDto> CreateAsync(CreateWarehouseRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseRoute, request, cancellationToken);

        return await ReadWarehouseAsync(response, cancellationToken);
    }

    public async Task<WarehouseDto> UpdateAsync(Guid id, UpdateWarehouseRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseRoute}/{id}", request, cancellationToken);

        return await ReadWarehouseAsync(response, cancellationToken);
    }

    public async Task<WarehouseDto> ActivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PatchAsync($"{BaseRoute}/{id}/activate", null, cancellationToken);

        return await ReadWarehouseAsync(response, cancellationToken);
    }

    public async Task<WarehouseDto> DeactivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PatchAsync($"{BaseRoute}/{id}/deactivate", null, cancellationToken);

        return await ReadWarehouseAsync(response, cancellationToken);
    }

    private static async Task<WarehouseDto> ReadWarehouseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var warehouse = await response.Content.ReadFromJsonAsync<WarehouseDto>(cancellationToken: cancellationToken);

        return warehouse ?? throw new ApiException((int)response.StatusCode, "The API returned an empty response.");
    }
}
