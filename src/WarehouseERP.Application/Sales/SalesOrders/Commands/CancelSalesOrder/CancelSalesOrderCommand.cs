namespace WarehouseERP.Application.Sales.SalesOrders.Commands.CancelSalesOrder;

public sealed class CancelSalesOrderCommand
{
    public required Guid SalesOrderId { get; init; }
}
