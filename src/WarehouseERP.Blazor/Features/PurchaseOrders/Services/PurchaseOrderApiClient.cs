using System.Net.Http.Json;
using WarehouseERP.Blazor.Infrastructure.Http;
using WarehouseERP.Shared.Contracts.PurchaseOrders;

namespace WarehouseERP.Blazor.Features.PurchaseOrders.Services;

public sealed class PurchaseOrderApiClient : IPurchaseOrderApiClient
{
    private const string BaseRoute = "api/purchase-orders";

    private readonly HttpClient _httpClient;

    public PurchaseOrderApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<PurchaseOrderDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(BaseRoute, cancellationToken);
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var purchaseOrders = await response.Content.ReadFromJsonAsync<IReadOnlyList<PurchaseOrderDto>>(cancellationToken: cancellationToken);

        return purchaseOrders ?? [];
    }

    public async Task<PurchaseOrderDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{BaseRoute}/{id}", cancellationToken);

        return await ReadPurchaseOrderAsync(response, cancellationToken);
    }

    public async Task<IReadOnlyList<PurchaseOrderDto>> GetBySupplierIdAsync(Guid supplierId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"api/suppliers/{supplierId}/purchase-orders", cancellationToken);
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var purchaseOrders = await response.Content.ReadFromJsonAsync<IReadOnlyList<PurchaseOrderDto>>(cancellationToken: cancellationToken);

        return purchaseOrders ?? [];
    }

    public async Task<PurchaseOrderDto> CreateAsync(CreatePurchaseOrderRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseRoute, request, cancellationToken);

        return await ReadPurchaseOrderAsync(response, cancellationToken);
    }

    public async Task<PurchaseOrderDto> AddLineAsync(Guid id, AddPurchaseOrderLineRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync($"{BaseRoute}/{id}/lines", request, cancellationToken);

        return await ReadPurchaseOrderAsync(response, cancellationToken);
    }

    public async Task<PurchaseOrderDto> UpdateLineAsync(Guid id, Guid productId, UpdatePurchaseOrderLineRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseRoute}/{id}/lines/{productId}", request, cancellationToken);

        return await ReadPurchaseOrderAsync(response, cancellationToken);
    }

    public async Task<PurchaseOrderDto> RemoveLineAsync(Guid id, Guid productId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync($"{BaseRoute}/{id}/lines/{productId}", cancellationToken);

        return await ReadPurchaseOrderAsync(response, cancellationToken);
    }

    public async Task<PurchaseOrderDto> SubmitAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsync($"{BaseRoute}/{id}/submit", null, cancellationToken);

        return await ReadPurchaseOrderAsync(response, cancellationToken);
    }

    public async Task<PurchaseOrderDto> CancelAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsync($"{BaseRoute}/{id}/cancel", null, cancellationToken);

        return await ReadPurchaseOrderAsync(response, cancellationToken);
    }

    public async Task<PurchaseOrderDto> ReceiveLineAsync(Guid id, Guid productId, ReceivePurchaseOrderLineRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync($"{BaseRoute}/{id}/lines/{productId}/receive", request, cancellationToken);

        return await ReadPurchaseOrderAsync(response, cancellationToken);
    }

    private static async Task<PurchaseOrderDto> ReadPurchaseOrderAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var purchaseOrder = await response.Content.ReadFromJsonAsync<PurchaseOrderDto>(cancellationToken: cancellationToken);

        return purchaseOrder ?? throw new ApiException((int)response.StatusCode, "The API returned an empty response.");
    }
}
