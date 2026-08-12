using WarehouseERP.Domain.Sales;

namespace WarehouseERP.Application.Sales.SalesOrders;

public sealed class SalesOrderLineDto
{
    public required Guid Id { get; init; }
    public required Guid SalesOrderId { get; init; }
    public required Guid ProductId { get; init; }
    public required int QuantityOrdered { get; init; }
    public required int QuantityFulfilled { get; init; }
    public required decimal UnitPrice { get; init; }

    public static SalesOrderLineDto FromDomain(SalesOrderLine line)
    {
        return new SalesOrderLineDto
        {
            Id = line.Id,
            SalesOrderId = line.SalesOrderId,
            ProductId = line.ProductId,
            QuantityOrdered = line.QuantityOrdered,
            QuantityFulfilled = line.QuantityFulfilled,
            UnitPrice = line.UnitPrice
        };
    }
}
