using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Application.Inventory.InventoryItems;

public interface IInventoryItemRepository
{
    Task<InventoryItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<InventoryItem>> GetAllAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<InventoryItem>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken);

    Task<IReadOnlyList<InventoryItem>> GetByStorageLocationIdAsync(Guid storageLocationId, CancellationToken cancellationToken);

    Task<InventoryItem?> GetByProductIdAndStorageLocationIdAsync(Guid productId, Guid storageLocationId, CancellationToken cancellationToken);

    // Tracks the entity for the caller's Unit of Work to commit; does not save changes itself.
    Task AddAsync(InventoryItem inventoryItem, CancellationToken cancellationToken);

    // Tracks the entity for the caller's Unit of Work to commit; does not save changes itself.
    Task UpdateAsync(InventoryItem inventoryItem, CancellationToken cancellationToken);
}
