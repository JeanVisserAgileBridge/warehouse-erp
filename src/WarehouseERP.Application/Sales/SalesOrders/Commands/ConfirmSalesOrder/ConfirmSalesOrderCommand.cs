namespace WarehouseERP.Application.Sales.SalesOrders.Commands.ConfirmSalesOrder;

public sealed class ConfirmSalesOrderCommand
{
    public required Guid SalesOrderId { get; init; }
}
