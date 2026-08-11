namespace WarehouseERP.Application.Inventory.InventoryItems.Commands.AdjustStock;

public sealed class AdjustStockCommand
{
    public required Guid InventoryItemId { get; init; }
    public required int NewQuantityOnHand { get; init; }
    public string? Reference { get; init; }
}
