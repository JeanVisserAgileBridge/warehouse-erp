using WarehouseERP.Shared.Contracts.Inventory;

namespace WarehouseERP.Blazor.Features.Inventory.Services;

public interface IInventoryApiClient
{
    Task<IReadOnlyList<InventoryItemDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<InventoryItemDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<InventoryItemDto>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<InventoryItemDto>> GetByStorageLocationIdAsync(Guid storageLocationId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<StockMovementDto>> GetMovementsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<InventoryItemDto> CreateAsync(CreateInventoryItemRequest request, CancellationToken cancellationToken = default);

    Task<InventoryItemDto> ReceiveAsync(Guid id, ReceiveStockRequest request, CancellationToken cancellationToken = default);

    Task<InventoryItemDto> IssueAsync(Guid id, IssueStockRequest request, CancellationToken cancellationToken = default);

    Task<InventoryItemDto> AdjustAsync(Guid id, AdjustStockRequest request, CancellationToken cancellationToken = default);

    Task<InventoryItemDto> ChangeReorderLevelAsync(Guid id, ChangeReorderLevelRequest request, CancellationToken cancellationToken = default);
}
