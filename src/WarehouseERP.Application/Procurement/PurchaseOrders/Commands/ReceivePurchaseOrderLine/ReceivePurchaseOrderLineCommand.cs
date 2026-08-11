namespace WarehouseERP.Application.Procurement.PurchaseOrders.Commands.ReceivePurchaseOrderLine;

public sealed class ReceivePurchaseOrderLineCommand
{
    public required Guid PurchaseOrderId { get; init; }
    public required Guid ProductId { get; init; }
    public required int Quantity { get; init; }
    public required Guid StorageLocationId { get; init; }
    public string? Reference { get; init; }
}
