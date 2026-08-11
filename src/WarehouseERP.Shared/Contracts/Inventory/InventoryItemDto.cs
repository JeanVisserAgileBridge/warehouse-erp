namespace WarehouseERP.Shared.Contracts.Inventory;

public sealed class InventoryItemDto
{
    public required Guid Id { get; init; }
    public required Guid ProductId { get; init; }
    public required Guid StorageLocationId { get; init; }
    public required int QuantityOnHand { get; init; }
    public required int ReorderLevel { get; init; }
    public required DateTime UpdatedAt { get; init; }
}
