namespace WarehouseERP.Shared.Contracts.PurchaseOrders;

public sealed class ReceivePurchaseOrderLineRequest
{
    public required int Quantity { get; init; }
    public required Guid StorageLocationId { get; init; }
    public string? Reference { get; init; }
}
