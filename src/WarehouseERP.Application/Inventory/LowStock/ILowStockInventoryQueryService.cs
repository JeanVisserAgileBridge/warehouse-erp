namespace WarehouseERP.Application.Inventory.LowStock;

public interface ILowStockInventoryQueryService
{
    Task<IReadOnlyList<LowStockInventoryItem>> GetLowStockItemsAsync(CancellationToken cancellationToken);
}
