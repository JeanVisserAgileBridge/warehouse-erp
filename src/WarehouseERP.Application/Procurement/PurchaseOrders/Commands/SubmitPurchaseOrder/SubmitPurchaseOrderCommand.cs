namespace WarehouseERP.Application.Procurement.PurchaseOrders.Commands.SubmitPurchaseOrder;

public sealed class SubmitPurchaseOrderCommand
{
    public required Guid PurchaseOrderId { get; init; }
}
