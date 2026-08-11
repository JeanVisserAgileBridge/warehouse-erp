namespace WarehouseERP.Application.Inventory.LowStock;

public sealed class LowStockInventoryItem
{
    public Guid InventoryItemId { get; init; }

    public Guid ProductId { get; init; }

    public Guid StorageLocationId { get; init; }

    public int QuantityOnHand { get; init; }

    public int ReorderLevel { get; init; }
}
