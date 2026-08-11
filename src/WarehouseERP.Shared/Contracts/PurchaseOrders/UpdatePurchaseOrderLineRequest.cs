namespace WarehouseERP.Shared.Contracts.PurchaseOrders;

public sealed class UpdatePurchaseOrderLineRequest
{
    public required int QuantityOrdered { get; init; }
    public required decimal UnitPrice { get; init; }
}
