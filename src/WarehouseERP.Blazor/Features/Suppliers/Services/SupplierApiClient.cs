using System.Net.Http.Json;
using WarehouseERP.Blazor.Infrastructure.Http;
using WarehouseERP.Shared.Contracts.Suppliers;

namespace WarehouseERP.Blazor.Features.Suppliers.Services;

public sealed class SupplierApiClient : ISupplierApiClient
{
    private const string BaseRoute = "api/suppliers";

    private readonly HttpClient _httpClient;

    public SupplierApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<SupplierDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(BaseRoute, cancellationToken);
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var suppliers = await response.Content.ReadFromJsonAsync<IReadOnlyList<SupplierDto>>(cancellationToken: cancellationToken);

        return suppliers ?? [];
    }

    public async Task<SupplierDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{BaseRoute}/{id}", cancellationToken);

        return await ReadSupplierAsync(response, cancellationToken);
    }

    public async Task<SupplierDto> CreateAsync(CreateSupplierRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseRoute, request, cancellationToken);

        return await ReadSupplierAsync(response, cancellationToken);
    }

    public async Task<SupplierDto> UpdateAsync(Guid id, UpdateSupplierRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseRoute}/{id}", request, cancellationToken);

        return await ReadSupplierAsync(response, cancellationToken);
    }

    public async Task<SupplierDto> ActivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PatchAsync($"{BaseRoute}/{id}/activate", null, cancellationToken);

        return await ReadSupplierAsync(response, cancellationToken);
    }

    public async Task<SupplierDto> DeactivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PatchAsync($"{BaseRoute}/{id}/deactivate", null, cancellationToken);

        return await ReadSupplierAsync(response, cancellationToken);
    }

    private static async Task<SupplierDto> ReadSupplierAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var supplier = await response.Content.ReadFromJsonAsync<SupplierDto>(cancellationToken: cancellationToken);

        return supplier ?? throw new ApiException((int)response.StatusCode, "The API returned an empty response.");
    }
}
