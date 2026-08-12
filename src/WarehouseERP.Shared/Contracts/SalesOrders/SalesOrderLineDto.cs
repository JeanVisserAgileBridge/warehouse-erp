namespace WarehouseERP.Shared.Contracts.SalesOrders;

public sealed class SalesOrderLineDto
{
    public required Guid Id { get; init; }
    public required Guid SalesOrderId { get; init; }
    public required Guid ProductId { get; init; }
    public required int QuantityOrdered { get; init; }
    public required int QuantityFulfilled { get; init; }
    public required decimal UnitPrice { get; init; }
}
