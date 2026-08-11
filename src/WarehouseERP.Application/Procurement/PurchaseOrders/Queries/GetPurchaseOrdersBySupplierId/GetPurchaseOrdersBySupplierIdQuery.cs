namespace WarehouseERP.Application.Procurement.PurchaseOrders.Queries.GetPurchaseOrdersBySupplierId;

public sealed class GetPurchaseOrdersBySupplierIdQuery
{
    public required Guid SupplierId { get; init; }
}
