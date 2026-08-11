namespace WarehouseERP.Application.Procurement.PurchaseOrders.Commands.AddPurchaseOrderLine;

public sealed class AddPurchaseOrderLineCommand
{
    public required Guid PurchaseOrderId { get; init; }
    public required Guid ProductId { get; init; }
    public required int QuantityOrdered { get; init; }
    public required decimal UnitPrice { get; init; }
}
