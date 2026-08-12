using WarehouseERP.Shared.Contracts.SalesOrders;
using ApplicationSalesOrderDto = WarehouseERP.Application.Sales.SalesOrders.SalesOrderDto;
using ApplicationSalesOrderLineDto = WarehouseERP.Application.Sales.SalesOrders.SalesOrderLineDto;
using DomainSalesOrderStatus = WarehouseERP.Domain.Sales.SalesOrderStatus;

namespace WarehouseERP.Api.Contracts.SalesOrders;

internal static class SalesOrderDtoExtensions
{
    public static SalesOrderDto ToContract(this ApplicationSalesOrderDto dto)
    {
        return new SalesOrderDto
        {
            Id = dto.Id,
            CustomerId = dto.CustomerId,
            OrderNumber = dto.OrderNumber,
            OrderDate = dto.OrderDate,
            Status = dto.Status.ToContract(),
            Notes = dto.Notes,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,
            Lines = dto.Lines.Select(ToContract).ToList()
        };
    }

    public static IReadOnlyList<SalesOrderDto> ToContract(this IReadOnlyList<ApplicationSalesOrderDto> dtos)
    {
        return dtos.Select(ToContract).ToList();
    }

    private static SalesOrderLineDto ToContract(this ApplicationSalesOrderLineDto dto)
    {
        return new SalesOrderLineDto
        {
            Id = dto.Id,
            SalesOrderId = dto.SalesOrderId,
            ProductId = dto.ProductId,
            QuantityOrdered = dto.QuantityOrdered,
            QuantityFulfilled = dto.QuantityFulfilled,
            UnitPrice = dto.UnitPrice
        };
    }

    private static SalesOrderStatus ToContract(this DomainSalesOrderStatus status) => status switch
    {
        DomainSalesOrderStatus.Draft => SalesOrderStatus.Draft,
        DomainSalesOrderStatus.Confirmed => SalesOrderStatus.Confirmed,
        DomainSalesOrderStatus.PartiallyFulfilled => SalesOrderStatus.PartiallyFulfilled,
        DomainSalesOrderStatus.Fulfilled => SalesOrderStatus.Fulfilled,
        DomainSalesOrderStatus.Cancelled => SalesOrderStatus.Cancelled,
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Unknown sales order status.")
    };
}
