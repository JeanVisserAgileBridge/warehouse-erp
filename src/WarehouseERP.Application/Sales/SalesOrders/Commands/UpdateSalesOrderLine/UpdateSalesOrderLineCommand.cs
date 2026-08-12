namespace WarehouseERP.Application.Sales.SalesOrders.Commands.UpdateSalesOrderLine;

public sealed class UpdateSalesOrderLineCommand
{
    public required Guid SalesOrderId { get; init; }
    public required Guid ProductId { get; init; }
    public required int QuantityOrdered { get; init; }
    public required decimal UnitPrice { get; init; }
}
