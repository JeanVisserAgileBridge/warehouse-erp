namespace WarehouseERP.Application.Inventory.InventoryItems.Commands.ChangeReorderLevel;

public sealed class ChangeReorderLevelCommand
{
    public required Guid InventoryItemId { get; init; }
    public required int ReorderLevel { get; init; }
}
