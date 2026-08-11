namespace WarehouseERP.Shared.Contracts.Inventory;

public sealed class StockMovementDto
{
    public required Guid Id { get; init; }
    public required Guid InventoryItemId { get; init; }
    public required StockMovementType MovementType { get; init; }
    public required int Quantity { get; init; }
    public string? Reference { get; init; }
    public required DateTime OccurredAt { get; init; }
}
