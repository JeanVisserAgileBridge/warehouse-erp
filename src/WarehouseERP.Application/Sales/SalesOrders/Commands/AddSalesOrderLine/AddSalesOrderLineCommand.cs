namespace WarehouseERP.Application.Sales.SalesOrders.Commands.AddSalesOrderLine;

public sealed class AddSalesOrderLineCommand
{
    public required Guid SalesOrderId { get; init; }
    public required Guid ProductId { get; init; }
    public required int QuantityOrdered { get; init; }
    public required decimal UnitPrice { get; init; }
}
