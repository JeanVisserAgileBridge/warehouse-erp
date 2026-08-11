using WarehouseERP.Domain.Procurement;

namespace WarehouseERP.Application.Procurement.PurchaseOrders;

public sealed class PurchaseOrderDto
{
    public required Guid Id { get; init; }
    public required Guid SupplierId { get; init; }
    public required string OrderNumber { get; init; }
    public required DateTime OrderDate { get; init; }
    public required PurchaseOrderStatus Status { get; init; }
    public string? Notes { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
    public required IReadOnlyList<PurchaseOrderLineDto> Lines { get; init; }

    public static PurchaseOrderDto FromDomain(PurchaseOrder purchaseOrder)
    {
        return new PurchaseOrderDto
        {
            Id = purchaseOrder.Id,
            SupplierId = purchaseOrder.SupplierId,
            OrderNumber = purchaseOrder.OrderNumber,
            OrderDate = purchaseOrder.OrderDate,
            Status = purchaseOrder.Status,
            Notes = purchaseOrder.Notes,
            CreatedAt = purchaseOrder.CreatedAt,
            UpdatedAt = purchaseOrder.UpdatedAt,
            Lines = purchaseOrder.Lines.Select(PurchaseOrderLineDto.FromDomain).ToList()
        };
    }
}
