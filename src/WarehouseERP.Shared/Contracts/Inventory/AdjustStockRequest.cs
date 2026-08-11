namespace WarehouseERP.Shared.Contracts.Inventory;

public sealed class AdjustStockRequest
{
    public required int NewQuantityOnHand { get; init; }
    public string? Reference { get; init; }
}
