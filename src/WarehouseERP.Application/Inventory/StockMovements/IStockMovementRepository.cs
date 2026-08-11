using WarehouseERP.Domain.Inventory;

namespace WarehouseERP.Application.Inventory.StockMovements;

public interface IStockMovementRepository
{
    Task<IReadOnlyList<StockMovement>> GetByInventoryItemIdAsync(Guid inventoryItemId, CancellationToken cancellationToken);

    // Tracks the entity for the caller's Unit of Work to commit; does not save changes itself.
    // StockMovement is immutable, so there is no corresponding UpdateAsync.
    Task AddAsync(StockMovement stockMovement, CancellationToken cancellationToken);
}
