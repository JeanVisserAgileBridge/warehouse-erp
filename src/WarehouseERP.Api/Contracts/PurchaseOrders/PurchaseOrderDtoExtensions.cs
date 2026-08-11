using WarehouseERP.Shared.Contracts.PurchaseOrders;
using ApplicationPurchaseOrderDto = WarehouseERP.Application.Procurement.PurchaseOrders.PurchaseOrderDto;
using ApplicationPurchaseOrderLineDto = WarehouseERP.Application.Procurement.PurchaseOrders.PurchaseOrderLineDto;
using DomainPurchaseOrderStatus = WarehouseERP.Domain.Procurement.PurchaseOrderStatus;

namespace WarehouseERP.Api.Contracts.PurchaseOrders;

internal static class PurchaseOrderDtoExtensions
{
    public static PurchaseOrderDto ToContract(this ApplicationPurchaseOrderDto dto)
    {
        return new PurchaseOrderDto
        {
            Id = dto.Id,
            SupplierId = dto.SupplierId,
            OrderNumber = dto.OrderNumber,
            OrderDate = dto.OrderDate,
            Status = dto.Status.ToContract(),
            Notes = dto.Notes,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,
            Lines = dto.Lines.Select(ToContract).ToList()
        };
    }

    public static IReadOnlyList<PurchaseOrderDto> ToContract(this IReadOnlyList<ApplicationPurchaseOrderDto> dtos)
    {
        return dtos.Select(ToContract).ToList();
    }

    private static PurchaseOrderLineDto ToContract(this ApplicationPurchaseOrderLineDto dto)
    {
        return new PurchaseOrderLineDto
        {
            Id = dto.Id,
            PurchaseOrderId = dto.PurchaseOrderId,
            ProductId = dto.ProductId,
            QuantityOrdered = dto.QuantityOrdered,
            QuantityReceived = dto.QuantityReceived,
            UnitPrice = dto.UnitPrice
        };
    }

    private static PurchaseOrderStatus ToContract(this DomainPurchaseOrderStatus status) => status switch
    {
        DomainPurchaseOrderStatus.Draft => PurchaseOrderStatus.Draft,
        DomainPurchaseOrderStatus.Submitted => PurchaseOrderStatus.Submitted,
        DomainPurchaseOrderStatus.PartiallyReceived => PurchaseOrderStatus.PartiallyReceived,
        DomainPurchaseOrderStatus.Received => PurchaseOrderStatus.Received,
        DomainPurchaseOrderStatus.Cancelled => PurchaseOrderStatus.Cancelled,
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Unknown purchase order status.")
    };
}
