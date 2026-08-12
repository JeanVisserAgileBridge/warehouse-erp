namespace WarehouseERP.Application.Sales.SalesOrders.Commands.FulfilSalesOrderLine;

public sealed class FulfilSalesOrderLineCommand
{
    public required Guid SalesOrderId { get; init; }
    public required Guid ProductId { get; init; }
    public required int Quantity { get; init; }
    public required Guid StorageLocationId { get; init; }
    public string? Reference { get; init; }
}
