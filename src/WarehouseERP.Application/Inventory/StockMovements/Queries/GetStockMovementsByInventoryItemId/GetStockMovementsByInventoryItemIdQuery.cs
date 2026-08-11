namespace WarehouseERP.Application.Inventory.StockMovements.Queries.GetStockMovementsByInventoryItemId;

public sealed class GetStockMovementsByInventoryItemIdQuery
{
    public required Guid InventoryItemId { get; init; }
}
