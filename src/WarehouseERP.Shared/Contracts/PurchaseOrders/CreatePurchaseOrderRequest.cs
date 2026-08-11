namespace WarehouseERP.Shared.Contracts.PurchaseOrders;

public sealed class CreatePurchaseOrderRequest
{
    public required Guid SupplierId { get; init; }
    public required string OrderNumber { get; init; }
    public required DateTime OrderDate { get; init; }
    public string? Notes { get; init; }
}
