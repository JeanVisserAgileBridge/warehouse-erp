namespace WarehouseERP.Application.Inventory.InventoryItems.Commands.ReceiveStock;

public sealed class ReceiveStockCommand
{
    public required Guid InventoryItemId { get; init; }
    public required int Quantity { get; init; }
    public string? Reference { get; init; }
}
