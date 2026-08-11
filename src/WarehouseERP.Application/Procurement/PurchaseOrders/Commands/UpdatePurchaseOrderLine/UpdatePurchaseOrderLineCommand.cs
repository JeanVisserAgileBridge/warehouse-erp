namespace WarehouseERP.Application.Procurement.PurchaseOrders.Commands.UpdatePurchaseOrderLine;

public sealed class UpdatePurchaseOrderLineCommand
{
    public required Guid PurchaseOrderId { get; init; }
    public required Guid ProductId { get; init; }
    public required int QuantityOrdered { get; init; }
    public required decimal UnitPrice { get; init; }
}
