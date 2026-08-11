namespace WarehouseERP.Shared.Contracts.PurchaseOrders;

public sealed class PurchaseOrderLineDto
{
    public required Guid Id { get; init; }
    public required Guid PurchaseOrderId { get; init; }
    public required Guid ProductId { get; init; }
    public required int QuantityOrdered { get; init; }
    public required int QuantityReceived { get; init; }
    public required decimal UnitPrice { get; init; }
}
