namespace WarehouseERP.Shared.Contracts.PurchaseOrders;

public sealed class AddPurchaseOrderLineRequest
{
    public required Guid ProductId { get; init; }
    public required int QuantityOrdered { get; init; }
    public required decimal UnitPrice { get; init; }
}
