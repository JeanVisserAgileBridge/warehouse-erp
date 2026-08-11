namespace WarehouseERP.Application.Procurement.PurchaseOrders.Commands.CreatePurchaseOrder;

public sealed class CreatePurchaseOrderCommand
{
    public required Guid SupplierId { get; init; }
    public required string OrderNumber { get; init; }
    public required DateTime OrderDate { get; init; }
    public string? Notes { get; init; }
}
