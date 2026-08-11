namespace WarehouseERP.Application.Inventory.InventoryItems.Commands.CreateInventoryItem;

public sealed class CreateInventoryItemCommand
{
    public required Guid ProductId { get; init; }
    public required Guid StorageLocationId { get; init; }
    public int QuantityOnHand { get; init; }
    public int ReorderLevel { get; init; }
}
