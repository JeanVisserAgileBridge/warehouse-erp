namespace WarehouseERP.Application.Procurement.PurchaseOrders.Commands.CancelPurchaseOrder;

public sealed class CancelPurchaseOrderCommand
{
    public required Guid PurchaseOrderId { get; init; }
}
