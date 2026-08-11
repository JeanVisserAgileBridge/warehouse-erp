using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Application.Procurement.PurchaseOrders;

public sealed class PurchaseOrderLineDto
{
    public required Guid Id { get; init; }
    public required Guid PurchaseOrderId { get; init; }
    public required Guid ProductId { get; init; }
    public required int QuantityOrdered { get; init; }
    public required int QuantityReceived { get; init; }
    public required decimal UnitPrice { get; init; }

    public static PurchaseOrderLineDto FromDomain(PurchaseOrderLine line)
    {
        return new PurchaseOrderLineDto
        {
            Id = line.Id,
            PurchaseOrderId = line.PurchaseOrderId,
            ProductId = line.ProductId,
            QuantityOrdered = line.QuantityOrdered,
            QuantityReceived = line.QuantityReceived,
            UnitPrice = line.UnitPrice
        };
    }
}
