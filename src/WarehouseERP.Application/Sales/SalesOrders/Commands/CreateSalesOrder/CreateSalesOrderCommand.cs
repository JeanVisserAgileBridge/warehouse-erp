namespace WarehouseERP.Application.Sales.SalesOrders.Commands.CreateSalesOrder;

public sealed class CreateSalesOrderCommand
{
    public required Guid CustomerId { get; init; }
    public required string OrderNumber { get; init; }
    public required DateTime OrderDate { get; init; }
    public string? Notes { get; init; }
}
