namespace WarehouseERP.Application.Sales.SalesOrders.Commands.RemoveSalesOrderLine;

public sealed class RemoveSalesOrderLineCommand
{
    public required Guid SalesOrderId { get; init; }
    public required Guid ProductId { get; init; }
}
