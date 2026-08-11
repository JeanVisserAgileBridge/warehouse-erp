namespace WarehouseERP.Application.Procurement.PurchaseOrders.Commands.RemovePurchaseOrderLine;

public sealed class RemovePurchaseOrderLineCommand
{
    public required Guid PurchaseOrderId { get; init; }
    public required Guid ProductId { get; init; }
}
