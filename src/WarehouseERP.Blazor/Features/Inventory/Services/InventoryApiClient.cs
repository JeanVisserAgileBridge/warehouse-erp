using System.Net.Http.Json;
using WarehouseERP.Blazor.Infrastructure.Http;
using WarehouseERP.Shared.Contracts.Inventory;

namespace WarehouseERP.Blazor.Features.Inventory.Services;

public sealed class InventoryApiClient : IInventoryApiClient
{
    private const string BaseRoute = "api/inventory";

    private readonly HttpClient _httpClient;

    public InventoryApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<InventoryItemDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(BaseRoute, cancellationToken);
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var inventoryItems = await response.Content.ReadFromJsonAsync<IReadOnlyList<InventoryItemDto>>(cancellationToken: cancellationToken);

        return inventoryItems ?? [];
    }

    public async Task<InventoryItemDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{BaseRoute}/{id}", cancellationToken);

        return await ReadInventoryItemAsync(response, cancellationToken);
    }

    public async Task<IReadOnlyList<InventoryItemDto>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{BaseRoute}/product/{productId}", cancellationToken);
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var inventoryItems = await response.Content.ReadFromJsonAsync<IReadOnlyList<InventoryItemDto>>(cancellationToken: cancellationToken);

        return inventoryItems ?? [];
    }

    public async Task<IReadOnlyList<InventoryItemDto>> GetByStorageLocationIdAsync(Guid storageLocationId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{BaseRoute}/storage-location/{storageLocationId}", cancellationToken);
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var inventoryItems = await response.Content.ReadFromJsonAsync<IReadOnlyList<InventoryItemDto>>(cancellationToken: cancellationToken);

        return inventoryItems ?? [];
    }

    public async Task<IReadOnlyList<StockMovementDto>> GetMovementsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{BaseRoute}/{id}/movements", cancellationToken);
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var stockMovements = await response.Content.ReadFromJsonAsync<IReadOnlyList<StockMovementDto>>(cancellationToken: cancellationToken);

        return stockMovements ?? [];
    }

    public async Task<InventoryItemDto> CreateAsync(CreateInventoryItemRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseRoute, request, cancellationToken);

        return await ReadInventoryItemAsync(response, cancellationToken);
    }

    public async Task<InventoryItemDto> ReceiveAsync(Guid id, ReceiveStockRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync($"{BaseRoute}/{id}/receive", request, cancellationToken);

        return await ReadInventoryItemAsync(response, cancellationToken);
    }

    public async Task<InventoryItemDto> IssueAsync(Guid id, IssueStockRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync($"{BaseRoute}/{id}/issue", request, cancellationToken);

        return await ReadInventoryItemAsync(response, cancellationToken);
    }

    public async Task<InventoryItemDto> AdjustAsync(Guid id, AdjustStockRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync($"{BaseRoute}/{id}/adjust", request, cancellationToken);

        return await ReadInventoryItemAsync(response, cancellationToken);
    }

    public async Task<InventoryItemDto> ChangeReorderLevelAsync(Guid id, ChangeReorderLevelRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PatchAsJsonAsync($"{BaseRoute}/{id}/reorder-level", request, cancellationToken);

        return await ReadInventoryItemAsync(response, cancellationToken);
    }

    private static async Task<InventoryItemDto> ReadInventoryItemAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        await response.EnsureSuccessOrThrowApiExceptionAsync(cancellationToken);

        var inventoryItem = await response.Content.ReadFromJsonAsync<InventoryItemDto>(cancellationToken: cancellationToken);

        return inventoryItem ?? throw new ApiException((int)response.StatusCode, "The API returned an empty response.");
    }
}
