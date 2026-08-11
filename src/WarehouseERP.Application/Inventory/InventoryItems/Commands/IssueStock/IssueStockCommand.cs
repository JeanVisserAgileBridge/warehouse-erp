namespace WarehouseERP.Application.Inventory.InventoryItems.Commands.IssueStock;

public sealed class IssueStockCommand
{
    public required Guid InventoryItemId { get; init; }
    public required int Quantity { get; init; }
    public string? Reference { get; init; }
}
