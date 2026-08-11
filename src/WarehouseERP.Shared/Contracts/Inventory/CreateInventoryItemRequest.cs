namespace WarehouseERP.Shared.Contracts.Inventory;

public sealed class CreateInventoryItemRequest
{
    public required Guid ProductId { get; init; }
    public required Guid StorageLocationId { get; init; }
    public int QuantityOnHand { get; init; }
    public int ReorderLevel { get; init; }
}
